using System.Threading.Tasks;

namespace CodeAssignment.Crawler.Abstractions;

// common interface for crawlers
public interface ICrawler
{
    Task<string> GetTitle(string uri);
}