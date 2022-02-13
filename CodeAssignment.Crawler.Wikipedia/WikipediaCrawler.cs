using AngleSharp;
using CodeAssignment.Crawler.Abstractions;

namespace CodeAssignment.Crawler.Wikipedia;

// Wikipedia crawler which is capable to work with Wikipedia URLs only
public class WikipediaCrawler : ICrawler
{
    private const string TitleTag = "title";

    private readonly IBrowsingContext _browsingContext;
    private readonly ILogger<WikipediaCrawler> _logger;

    public WikipediaCrawler(IBrowsingContext browsingContext, ILogger<WikipediaCrawler> logger)
    {
        _browsingContext = browsingContext;
        _logger = logger;
    }

    public async Task<string> GetTitle(string uri)
    {
        _logger.LogInformation("Getting title for {uri}", uri);

        if (!uri.Contains("wikipedia.org"))
        {
            _logger.LogInformation("{uri} is not Wikipedia's page", uri);
            throw new InvalidOperationException("Not supported URL");
        }
        
        // https://anglesharp.github.io library is used to parse HTML 
        using var document = await _browsingContext.OpenAsync(uri);
        var title = document.QuerySelector(TitleTag).TextContent;
        
        _logger.LogInformation("{title} has been resolved for {uri}", title, uri);
        
        return title;
    }
}