using System.Threading.Tasks;

namespace CodeAssignment.Crawler.Universal.RestClient;

public interface IUniversalCrawlerClient
{
    Task<string> GetTitle(string uri);
}