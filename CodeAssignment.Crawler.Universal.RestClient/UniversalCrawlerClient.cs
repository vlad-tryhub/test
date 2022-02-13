using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeAssignment.Crawler.Universal.RestClient;

// REST client to access Universal crawler API
public class UniversalCrawlerClient : IUniversalCrawlerClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<UniversalCrawlerClient> _logger;
    private readonly UniversalCrawlerOptions _options;

    public UniversalCrawlerClient(IHttpClientFactory httpClientFactory, ILogger<UniversalCrawlerClient> logger, IOptions<UniversalCrawlerOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> GetTitle(string uri)
    {
        _logger.LogInformation("Requesting universal crawler to get title for {uri}", uri);
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(GetRequestAddress(uri));
        
        if (response.IsSuccessStatusCode)
        {
            var title = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Request successfully finished with {title}", title);
            return title;
        }
        
        _logger.LogInformation("Request failed with {code}", response.StatusCode);

        // TODO: properly react to failed requests, collect metrics and alert
        throw new Exception("Request failed");
    }

    private string GetRequestAddress(string uri) =>
        $"{_options.Address}/crawl?uri={WebUtility.UrlEncode(uri)}";
}