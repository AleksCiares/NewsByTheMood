using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.ScrapeProvider.Abstract
{
    /// <summary>
    /// Service for load articles to database
    /// </summary>
    public interface IArticleScrapeService
    {
        public Task<IEnumerable<Article>> ScrapeLatestBySourceAsync(Source source);
        public Task<Article> ScrapeAsync(Source source, string articleUrl);
    }
}
