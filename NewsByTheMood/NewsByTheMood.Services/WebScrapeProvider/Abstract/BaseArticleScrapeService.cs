using System.Text;
using AngleSharp.Dom;
using WebScraper.Core.Parsers.Abstract;
using WebScraper.Core.Parsers.Implement;
using WebScraper.Core.Settings;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.WebScrapeProvider.Models;
using AngleSharp;
using AngleSharp.Html;


namespace NewsByTheMood.Services.WebScrapeProvider.Abstract
{
    public abstract class BaseArticleScrapeService : IArticleScrapeService
    {
        protected readonly WebLoaderSettings _loadersettings;
        private readonly IMarkupFormatter _htmlMarkupFormatter;

        public BaseArticleScrapeService(WebLoaderSettings loadersettings) 
        {
            this._loadersettings = loadersettings;
            this._htmlMarkupFormatter = new PrettyMarkupFormatter();
            //this._htmlMarkupFormatter = new MyMarkupFormatter();
        }

        public abstract Task<List<string>> GetArticlesUrlsAsync(Source source);

        public abstract Task<List<ArticleScrapeModel>> GetArticlesAsync(Source source, string[] articlesUrls);

        protected List<string> ParseArticleUrls(Source source, string articleCollectionPage)
        {
            List<string> articleUrls = new List<string>();

            using (IDocumentParser<IElement> documentParser = new MyHtmlParser(articleCollectionPage))
            {
                var articlesCollections = documentParser.SelectAllFromDocument(source.ArticleCollectionsPath);

                List<IElement> articles = new List<IElement>();
                foreach (var articleCollection in articlesCollections)
                {
                    articles.AddRange(articleCollection.QuerySelectorAll(source.ArticleItemPath));
                }

                string? articleUrl = null;
                foreach (var article in articles)
                {
                    articleUrl = article.QuerySelector(source.ArticleUrlPath)?.GetAttribute("href");
                    if (articleUrl != null)
                    {
                        if (!Uri.IsWellFormedUriString(articleUrl, uriKind: UriKind.Absolute))
                        {
                            articleUrl = new Uri(new Uri(source.Url), articleUrl).ToString();
                        }
                        articleUrls.Add(articleUrl);
                    }
                }
            }
            return articleUrls;
        }

        protected ArticleScrapeModel ParseArticle(Source source, string articlePage)
        {
            ArticleScrapeModel article = new ArticleScrapeModel();
            using (IDocumentParser<IElement> documentParser = new MyHtmlParser(articlePage))
            {
                // article title
                article.Title = documentParser.SelectFromDocument(source.ArticleTitlePath)?.TextContent ?? "No article title";

                // article preview
                if (source.ArticlePreviewImgPath != null)
                {
                    article.PreviewImgUrl = documentParser.SelectFromDocument(source.ArticlePreviewImgPath)?.GetAttribute("src");
                }

                // article body
                var articleBodyCollections = documentParser.SelectAllFromDocument(source.ArticleBodyCollectionsPath);
                List<IElement> bodies = new List<IElement>();
                foreach (var articleBodyCollection in articleBodyCollections)
                {
                    bodies.AddRange(articleBodyCollection.QuerySelectorAll(source.ArticleBodyItemPath));
                }
                StringBuilder body = new StringBuilder();
                foreach (var bodyElement in bodies)
                {
                    body.Append(bodyElement.ToHtml(this._htmlMarkupFormatter));
                }
                article.Body = body.ToString();


                // article pudlish date
                if (source.ArticlePdatePath != null)
                {
                    article.PublishDate = documentParser.SelectFromDocument(source.ArticlePdatePath)?.TextContent;
                }

                // article tags
                if (source.ArticleTagPath != null)
                {
                    List<string> tags = new List<string>();
                    var tagsElements = documentParser.SelectAllFromDocument(source.ArticleTagPath);
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
