using System.Text;
using AngleSharp.Dom;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.WebScrapeProvider.Model;
using WebScraper.Core.Loaders;
using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Loaders.Implement;
using WebScraper.Core.Parsers.Abstract;
using WebScraper.Core.Parsers.Implement;

namespace NewsByTheMood.Services.WebScrapeProvider.Implement
{
    public class ArticleScrapeService : IDisposable
    {
        private bool _disposed = false;
        private readonly Source _source;
        private readonly IWebLoader _webloader;
        private readonly IArticleService _articleService; 

        public ArticleScrapeService(Source source, IWebLoader webLoader, IArticleService articleService) 
        {
            this._source = source;
            this._webloader = webLoader;
            this._articleService = articleService;
        }

        ~ArticleScrapeService()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._webloader.Dispose();
                }
                this._disposed = true;
            }
        }

        public async Task LoadArticles()
        {
            var articleCollectionsPage = await this._webloader.LoadPageAsync(this._source.Url);
            var articlesUrls = this.GetArticleUrls(articleCollectionsPage);

            var articles = new List<ArticleScrapeModel>();
            foreach (var articleUrl in articlesUrls)
            {
                if (await this._articleService.IsExistByUrl(articleUrl))
                {
                    continue;
                }

                var articlePage = await this._webloader.LoadPageAsync(articleUrl);
                articles.Add(this.GetArticle(articlePage));
            }
        }

        private List<string> GetArticleUrls(string articleCollectionPage)
        {
            List<string> articleUrls = new List<string>();
            using (IDocumentParser<IElement> documentParser = new HtmlParser(articleCollectionPage))
            {
                var articlesCollections = documentParser.SelectAllFromDocument(this._source.ArticleCollectionsPath);
                List<IElement> articles = new List<IElement>();
                foreach (var articleCollection in articlesCollections)
                {
                    articles.AddRange(articleCollection.QuerySelectorAll(this._source.ArticleItemPath));
                }
                string? articleUrl = null;
                foreach (var article in articles)
                {
                    articleUrl = article.QuerySelector(this._source.ArticleUrlPath)?.GetAttribute("href");
                    if (articleUrl != null)
                    {
                        articleUrls.Add(articleUrl);
                    }
                }
            }
            return articleUrls;
        }

        private ArticleScrapeModel GetArticle(string articlePage)
        {
            ArticleScrapeModel article = new ArticleScrapeModel();
            using (IDocumentParser<IElement> documentParser = new HtmlParser(articlePage))
            {
                // article title
                article.Title = documentParser.SelectFromDocument(this._source.ArticleTitlePath)?.TextContent ?? "No article title";

                // article preview
                if (this._source.ArticlePreviewImgPath != null)
                {
                    article.PreviewImgUrl = documentParser.SelectFromDocument(this._source.ArticlePreviewImgPath)?.GetAttribute("src");
                }

                // article body
                var articleBodyCollections = documentParser.SelectAllFromDocument(this._source.ArticleBodyCollectionsPath);
                List<IElement> bodies = new List<IElement>();
                foreach (var articleBodyCollection in articleBodyCollections)
                {
                    bodies.AddRange(articleBodyCollection.QuerySelectorAll(this._source.ArticleBodyItemPath));
                }
                StringBuilder body = new StringBuilder();
                foreach (var bodyElement in bodies)
                {
                    body.Append(bodyElement.TextContent);
                }
                article.Body = body.ToString();


                // article pudlish date
                if (this._source.ArticlePdatePath != null)
                {
                    article.PublishDate = documentParser.SelectFromDocument(this._source.ArticlePdatePath)?.TextContent;
                }

                // article tags
                if (this._source.ArticleTagPath != null)
                {
                    List<string> tags = new List<string>();
                    var tagsElements = documentParser.SelectAllFromDocument(this._source.ArticleTagPath);
                    foreach (var tagElement in tagsElements)
                    {
                        tags.Add(tagElement.TextContent);
                    }
                    article.Tags = tags.ToArray();
                }
               
            }
            return article;
        }
    }
}
