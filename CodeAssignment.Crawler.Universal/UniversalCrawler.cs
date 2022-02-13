using System.Threading.Tasks;
using AngleSharp;
using CodeAssignment.Crawler.Abstractions;
using Microsoft.Extensions.Logging;

namespace CodeAssignment.Crawler.Universal;

// universal crawler which is capable to work with any web-site
public class UniversalCrawler : ICrawler
{
    private const string TitleTag = "title";

    private readonly IBrowsingContext _browsingContext;
    private readonly ILogger<UniversalCrawler> _logger;

    public UniversalCrawler(IBrowsingContext browsingContext, ILogger<UniversalCrawler> logger)
    {
        _browsingContext = browsingContext;
        _logger = logger;
    }

    public async Task<string> GetTitle(string uri)
    {
        _logger.LogInformation("Getting title for {uri}", uri);

        // Simulate some difference from Wikipedia crawler.
        // Let's say Universal crawler can handle any web-site but is slower than dedicated one.
        await Task.Delay(2000);
        
        // https://anglesharp.github.io library is used to parse HTML 
        using var document = await _browsingContext.OpenAsync(uri);
        var title = document.QuerySelector(TitleTag).TextContent;
        
        _logger.LogInformation("{title} has been resolved for {uri}", title, uri);
        
        return title;
    }
}