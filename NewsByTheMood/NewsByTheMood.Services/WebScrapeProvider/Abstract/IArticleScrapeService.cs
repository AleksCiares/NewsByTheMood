using NewsByTheMood.Services.WebScrapeProvider.Models;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.WebScrapeProvider.Abstract
{
    /// <summary>
    /// Interface for article scrape service
    /// </summary>
    public interface IArticleScrapeService
    {
        /// <summary>
        /// Get articles urls from a source page which contains a list of articles
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Task<List<string>> GetArticlesUrlsAsync(Source source);

        /// <summary>
        /// Get articles from a articles page
        /// </summary>
        /// <param name="source"></param>
        /// <param name="articlesUrls"></param>
        /// <returns></returns>
        public Task<List<ArticleScrapeModel>> GetArticlesAsync(Source source, string[] articlesUrls);

    }
}
