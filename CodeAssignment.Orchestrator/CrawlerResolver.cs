namespace CodeAssignment.Orchestrator;

// This class is responsible for detecting ideal crawler for provided URL
public class CrawlerResolver : ICrawlerResolver
{
    public Crawler ResolveIdealCrawler(string url)
    {
        if (url.Contains("wikipedia.org"))
            return Crawler.Wikipedia;
        
        return Crawler.Universal;
    }
}