using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Loaders.Implement;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.WebScrapeProvider.Abstract;
using NewsByTheMood.Services.WebScrapeProvider.Models;
using WebScraper.Settings;


namespace NewsByTheMood.Services.WebScrapeProvider.Implement
{
    /// <summary>
    /// Service for scraping articles from a static page
    /// </summary>
    class StaticArticleScrapeService : BaseArticleScrapeService
    {
        public StaticArticleScrapeService(LoaderSettings loadersettings) : 
            base(loadersettings)
        {
        }

        public async override Task<List<string>> GetArticlesUrlsAsync(Source source)
        {
            var page = "";
            using (ILoader _webloader = new StaticPageLoader(this._loadersettings))
            {
                page = await _webloader.LoadPageAsync(source.Url);
            }
            return this.ParseArticleUrls(source, page);
        }

        public async override Task<List<ArticleScrapeModel>> GetArticlesAsync(Source source, string[] articlesUrls)
        {
            var articles = new List<ArticleScrapeModel>();

            var page = "";
            foreach (var articleUrl in articlesUrls)
            {
                using (ILoader _webloader = new StaticPageLoader(this._loadersettings))
                {
                    page = await _webloader.LoadPageAsync(source.Url);
                }
                articles.Add(this.ParseArticle(source, page));
            }

            return articles;
        }
    }
}
