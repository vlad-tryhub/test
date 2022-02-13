using System;
using System.Linq;
using System.Threading.Tasks;
using CodeAssignment.Crawler.Universal.RestClient;
using CodeAssignment.Crawler.Wikipedia.RestClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CodeAssignment.Orchestrator.Controllers;

public class CrawlerOrchestratorController : ControllerBase
{
    private readonly ILogger<CrawlerOrchestratorController> _logger;
    private readonly ICrawlerResolver _crawlerResolver;
    private readonly IUniversalCrawlerClient _universalCrawlerClient;
    private readonly IWikipediaCrawlerClient _wikipediaCrawlerClient;
    // TODO: move data access logic into separate class-repository
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;

    private const string CollectionName = "titles";

    public CrawlerOrchestratorController(
        ILogger<CrawlerOrchestratorController> logger,
        ICrawlerResolver crawlerResolver,
        IUniversalCrawlerClient universalCrawlerClient,
        IWikipediaCrawlerClient wikipediaCrawlerClient,
        IMongoDatabase database,
        IConfiguration configuration)
    {
        _logger = logger;
        _crawlerResolver = crawlerResolver;
        _universalCrawlerClient = universalCrawlerClient;
        _wikipediaCrawlerClient = wikipediaCrawlerClient;
        _database = database;
        _configuration = configuration;
    }

    [HttpGet("read")]
    public async Task<IActionResult> Read([FromQuery] string? threshold)
    {
        _logger.LogInformation("Received request to read latest results with {threshold}", threshold);

        // TODO: move threshold normalization logic into separate class-provider
        if (!int.TryParse(threshold, out var parsedThreshold) || parsedThreshold == -1)
        {
            _logger.LogInformation("Using threshold from appsettings.json");
            parsedThreshold = Convert.ToInt32(_configuration["ReadResultsTimeThreshold"]);
        }

        var readFrom = DateTime.UtcNow.AddMinutes(-parsedThreshold);
        var cursor = await _database.GetCollection<TitleEntity>(CollectionName)
            .FindAsync(Builders<TitleEntity>.Filter.Gte(x => x.ProcessedAt, readFrom));

        var result = await cursor.ToListAsync();
        
        return Ok(result);
    }

    [HttpPost("crawl")]
    public async Task<string> Crawl([FromBody] UrlsToCrawlDto request)
    {
        _logger.LogInformation("Received {@request} to crawl URLs", request);

        var processingTasks = request.Urls.Select(ProcessUrl);
        var results = await Task.WhenAll(processingTasks);

        return results.All(result => result) ? "ok" : "error";
    }

    // TODO: move into separate class-service
    private async Task<bool> ProcessUrl(string url)
    {
        try
        {
            // We don't process URL if the result is already stored
            if (await IsUrlAlreadyProcessed(url))
            {
                _logger.LogInformation("Skipping {url} because it is already processed", url);
                return true;
            }
            
            _logger.LogInformation("Started processing {url}", url);

            // we need to identify which crawler agent fits best for given URL
            var idealCrawler = _crawlerResolver.ResolveIdealCrawler(url);
            _logger.LogInformation("Ideal crawler agent is {idealCrawler}", idealCrawler);

            var title = default(string);
            if (idealCrawler == Crawler.Universal)
            {
                title = await _universalCrawlerClient.GetTitle(url);
            }
            else if (idealCrawler == Crawler.Wikipedia)
            {
                title = await _wikipediaCrawlerClient.GetTitle(url);
            }

            await _database.GetCollection<TitleEntity>(CollectionName)
                .InsertOneAsync(new TitleEntity
                {
                    Title = title!,
                    Url = url,
                    ProcessedAt = DateTime.UtcNow,
                });

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // TODO: properly react to failed requests, collect metrics and alert
            return false;
        }
    }

    private async Task<bool> IsUrlAlreadyProcessed(string url)
    {
        var cursor = await _database.GetCollection<TitleEntity>(CollectionName)
            .FindAsync(Builders<TitleEntity>.Filter.Eq(x => x.Url, url));

        return await cursor.AnyAsync();
    }
}