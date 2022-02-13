using CodeAssignment.Crawler.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CodeAssignment.Crawler.Wikipedia.Controllers;

public class CrawlerController : ControllerBase
{
    private readonly ILogger<CrawlerController> _logger;
    private readonly ICrawler _crawler;

    public CrawlerController(ILogger<CrawlerController> logger, ICrawler crawler)
    {
        _logger = logger;
        _crawler = crawler;
    }

    [HttpGet("/crawl")]
    public async Task<string> Crawl([FromQuery] string uri)
    {
        _logger.LogInformation("Received request to crawl {uri}", uri);
        var result = await _crawler.GetTitle(uri);
        return result;
    }
}