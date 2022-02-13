namespace CodeAssignment.Orchestrator;

public interface ICrawlerResolver
{
    Crawler ResolveIdealCrawler(string url);
}