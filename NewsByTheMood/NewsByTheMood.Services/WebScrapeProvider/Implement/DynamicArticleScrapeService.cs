using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Loaders.Implement;
using WebScraper.Core.Settings;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.WebScrapeProvider.Abstract;
using NewsByTheMood.Services.WebScrapeProvider.Models;


namespace NewsByTheMood.Services.WebScrapeProvider.Implement
{
    /// <summary>
    /// Service for scraping articles from a dynamic page
    /// </summary>
    public class DynamicArticleScrapeService : BaseArticleScrapeService
    {
        public DynamicArticleScrapeService(WebLoaderSettings loadersettings) : 
            base(loadersettings)
        {
        }

        public async override Task<List<string>> GetArticlesUrlsAsync(Source source)
        {
            var page = "";
            using (IWebLoader _webloader = new DynamicPageLoader(this._loadersettings))
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
                using (IWebLoader _webloader = new DynamicPageLoader(this._loadersettings))
                {
                    page = await _webloader.LoadPageAsync(articleUrl);
                }

                var article = this.ParseArticle(source, page);
                article.Url = articleUrl;
                articles.Add(article);
            }

            return articles;
        }
    }
}
