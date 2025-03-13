using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.ScrapeProvider.Abstract
{
    /// <summary>
    /// Service for load articles to database
    /// </summary>
    public interface IArticleScrapeService
    {
        public Task LoadArticles(Source source);
        public Task LoadArticle(Source source, string articleUrl);
    }
}
