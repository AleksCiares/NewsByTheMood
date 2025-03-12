using System.Text;
using AngleSharp.Dom;
using WebScraper.Core.Parsers.Abstract;
using WebScraper.Core.Parsers.Implement;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.WebScrapeProvider.Models;
using AngleSharp;
using AngleSharp.Html;
using System.Text.RegularExpressions;
using Azure;
using WebScraper.Settings;


namespace NewsByTheMood.Services.WebScrapeProvider.Abstract
{
    public abstract class BaseArticleScrapeService : IArticleScrapeService
    {
        protected readonly LoaderSettings _loadersettings;
        private readonly IMarkupFormatter _htmlMarkupFormatter;

        public BaseArticleScrapeService(LoaderSettings loadersettings) 
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

            using (IDocumentParser<IElement> documentParser = new PrettyHtmlParser(articleCollectionPage))
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
            using (IDocumentParser<IElement> documentParser = new PrettyHtmlParser(articlePage))
            {
                // article title
                article.Title = documentParser.SelectFromDocument(source.ArticleTitlePath)?.TextContent ?? "No article title";

                // article preview
                article.PreviewImgUrl = GetArticlePreviewImage(documentParser, source);

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
                    var whiteSpaceLessTag = Regex.Replace(tag, @"\s+", ""); // доделать чтобы не удалялиь пробелы между словами

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

        private string GetArticlePreviewImage(IDocumentParser<IElement> documentParser, Source source)
        {
            var imgSrc = String.Empty;

            if (source.ArticlePreviewImgPath != null)
            {
                var src = documentParser.SelectFromDocument(source.ArticlePreviewImgPath)?.GetAttribute("src");
                if (src == null)
                {
                    var imgStyle = documentParser.SelectFromDocument(source.ArticlePreviewImgPath)?.GetAttribute("style");
                    if (imgStyle != null)
                    {
                        var regex = new Regex(@"(?<=(background-image:\surl\((""|'|\s))).+(?=((""|'|\s)\)))", RegexOptions.Compiled);
                        var matches = regex.Matches(imgStyle);

                        if (matches.Count > 0)
                        {
                            imgSrc = matches[0].Value;
                        }
                    }
                }
            }

            return imgSrc;
        }
    }
}
private DateTime? TryParseDate(string? publishDate)
{
    if (DateTime.TryParse(publishDate, out var date))
    {
        return date;
    }
    return null;
}