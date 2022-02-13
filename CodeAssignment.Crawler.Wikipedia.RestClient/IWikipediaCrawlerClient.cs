using System.Threading.Tasks;

namespace CodeAssignment.Crawler.Wikipedia.RestClient;

public interface IWikipediaCrawlerClient
{
    Task<string> GetTitle(string uri);
}