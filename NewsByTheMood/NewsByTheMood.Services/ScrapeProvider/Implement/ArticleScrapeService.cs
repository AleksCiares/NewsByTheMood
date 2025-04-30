using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.ScrapeProvider.Abstract;
using WebScraper;
using WebScraper.Settings;
using Source = NewsByTheMood.Data.Entities.Source;

namespace NewsByTheMood.Services.ScrapeProvider.Implement
{
    public class ArticleScrapeService : IArticleScrapeService
    {
        private readonly WebScrapeOptions _options;
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleScrapeService> _logger;

        public ArticleScrapeService(
            IOptions<WebScrapeOptions> options, 
            IArticleService articleService,
            ILogger<ArticleScrapeService> logger)
        {
            _options = options.Value;
            _articleService = articleService;
            _logger = logger;
        }

        public async Task<IEnumerable<Article>> ScrapeLatestBySourceAsync(Source source)
        {
            _logger.LogInformation($"Scraping latest articles from source. SourceName: {source.Name}, Id: {source.Id}");

            // create scraper
            var scraper = CreateScraper(source);

            // load page
            try
            {
                await scraper.GetPageAsync(source.Url);
                _logger.LogDebug($"Article container page loaded successfully from {source.Url}. SourceName: {source.Name}, Id: {source.Id}");
            }
            catch (Exception ex)
            {
                scraper.Dispose();
                throw new Exception($"Error while loading page from {source.Url}. SourceName: {source.Name}, Id: {source.Id}", ex);
            }

            // parse articles urls
            var articlesUrls = scraper.Parser.Init(source.ArticleCollectionsPath)
                .SelectAll(source.ArticleItemPath)
                .SelectAll(source.ArticleUrlPath)
                .GetAttributes("href");
            articlesUrls.Reverse();
            _logger.LogDebug($"Successfully parsed {articlesUrls.Count} articles urls from page. SourceName: {source.Name}, Id: {source.Id}.\n" +
                $"Parsed urls: {String.Join("\n", articlesUrls.ToArray())}");

            // remove duplicates
            for (var i = articlesUrls.Count - 1; i >= 0; i--)
            {
                if (!Uri.IsWellFormedUriString(articlesUrls[i], uriKind: UriKind.Absolute))
                {
                    articlesUrls[i] = new Uri(new Uri(source.Url), articlesUrls[i]).ToString();
                }

                if (await _articleService.IsExistsByUrlAsync(articlesUrls[i]))
                {
                    articlesUrls.RemoveAt(i);
                }
            }
            _logger.LogDebug($"Removed duplicates. {articlesUrls.Count} articles urls left from page. SourceName: {source.Name}, Id: {source.Id}.\n" +
                $"Lefted urls: {String.Join("\n", articlesUrls.ToArray())}");

            var articles = new List<Article>();
            foreach (var url in articlesUrls)
            {
                Article? article = null;
                try
                {
                    // load article page
                    await scraper.GetPageAsync(url);
                    _logger.LogDebug($"Article page loaded successfully from {url}. SourceName: {source.Name}, Id: {source.Id}");

                    // parse article
                    article = ParseArticle(source, scraper);
                    _logger.LogDebug($"Article parsed successfully from {url}. ArticleTitle: {article.Title}, SourceName: {source.Name}, Id: {source.Id}");

                    article.IsActive = true;
                    article.FailedLoaded = false;
                }
                catch(Exception ex)
                {
                    article = new Article();
                    article.Positivity = 0;
                    article.Rating = 0;
                    article.Title = "Error while loading article";
                    article.SourceId = source.Id;
                    article.FailedLoaded = true;
                    article.IsActive = false;

                    _logger.LogError(ex, $"Error while loading article from {url}. SourceName: {source.Name}, Id: {source.Id}");
                }

                article.Url = url;
                articles.Add(article);
            }

            _logger.LogInformation($"Scraped {articles.Count} articles from page. SourceName: {source.Name}, Id: {source.Id}.\n" +
                $"Parsed articles: {String.Join("\n", articles.Select(a => a.Url).ToArray())}");

            scraper.Dispose();
            return articles;
        }

        public async Task<Article> ScrapeAsync(Source source, string articleUrl)
        {
            _logger.LogInformation($"Scraping latest articles from source. SourceName: {source.Name}, Id: {source.Id}");

            // create scraper
            var scraper = CreateScraper(source);

            try
            {
                // load page
                await scraper.GetPageAsync(articleUrl);
                _logger.LogDebug($"Article page loaded successfully from {articleUrl}. SourceName: {source.Name}, Id: {source.Id}");
            }
            catch (Exception ex)
            {
                scraper.Dispose();
                throw new Exception($"Error while loading article from {articleUrl}. SourceName: {source.Name}, Id: {source.Id}", ex);
            }

            // parse article
            var article = ParseArticle(source, scraper);
            article.Url = articleUrl;
            article.IsActive = true;
            article.FailedLoaded = false;

            _logger.LogDebug($"Article parsed successfully from {articleUrl}. ArticleTitle: {article.Title}, SourceName: {source.Name}, Id: {source.Id}");

            scraper.Dispose();
            return article;
        }

        private PrettyScraper CreateScraper(Source source)
        {
            var rnd = new Random();

            var userAgent = _options.UserAgents[rnd.Next(0, _options.UserAgents.Length - 1)];
            ProxySettings? proxy = null;
            if (_options.UseProxies && _options.Proxies != null && _options.Proxies.Length > 0)
            {
                if (_options.UseIpRotation)
                {
                    proxy = _options.Proxies[rnd.Next(0, _options.Proxies.Length - 1)];
                }
                else
                {
                    proxy = _options.Proxies[0];
                }
            }

            var loaderSettings = new LoaderSettings
            {
                AcceptInsecureCertificates = source.AcceptInsecureCerts,
                UserAgent = userAgent,
                ProxySettings = proxy,
                SignalElement = source.PageElementLoaded,
                PageLoadTimeout = TimeSpan.FromSeconds(source.PageLoadTimeout),
                ElementLoadTimeout = TimeSpan.FromSeconds(source.ElementLoadTimeout)
            };

            var scraperSettings = new ScraperSettings()
            {
                IsDynamicSource = source.HasDynamicPage,
                LoaderSettings = loaderSettings
            };

            return new PrettyScraper(scraperSettings);
        }

        private Article ParseArticle(Source source, PrettyScraper scraper)
        {
            return new Article()
            {
                Title = GetTitle(source, scraper),
                PreviewImgUrl = GetPreviewImageUrl(source, scraper),
                PublishDate = GetPublishDate(source, scraper),
                Positivity = 0,
                Rating = 0,
                SourceId = source.Id,
                Tags = GetTags(source, scraper),
                Body = GetBody(source, scraper),
            };
        }

        private string GetTitle(Source source, PrettyScraper scraper)
        {
            var title = scraper.Parser.Init(source.ArticleTitlePath).TextContent().ElementAtOrDefault(0);
            return title ?? "No found title";
        }

        private string? GetPreviewImageUrl(Source source, PrettyScraper scraper)
        {
            if (source.ArticlePreviewImgPath.IsNullOrEmpty())
            {
                return null;
            }

            var path = scraper.Parser.Init(source.ArticlePreviewImgPath!).GetAttribute("src");
            if (path.IsNullOrEmpty())
            {
                var imgStyle = scraper.Parser.Init(source.ArticlePreviewImgPath!).GetAttribute("style");
                if (!imgStyle.IsNullOrEmpty())
                {
                    var regex = new Regex(@"(?<=(background-image:\surl\((""|'|\s))).+(?=((""|'|\s)\)))", RegexOptions.Compiled);
                    var matches = regex.Matches(imgStyle!);
                    if (matches.Count > 0)
                    {
                        path = matches[0].Value;
                    }
                }
            }
            if (path.IsNullOrEmpty())
            {
                path = scraper.Parser.Init(source.ArticlePreviewImgPath!).GetAttribute("href");
            }

            if (path != null && !Uri.IsWellFormedUriString(path, uriKind: UriKind.Absolute))
            {
                path = new Uri(new Uri(source.Url), path).ToString();
            }

            return path;
        }

        // TODO: change article publish date from local service to UTC via source local time
        private DateTime? GetPublishDate(Source source, PrettyScraper scraper)
        {
            if (source.ArticlePdatePath.IsNullOrEmpty())
            {
                return null;
            }

            var plainDate = scraper.Parser.Init(source.ArticlePdatePath!).TextContent().ElementAtOrDefault(0);
            if (DateTime.TryParse(plainDate, out var date))
            {
                return date;
            }
            return null;
        }

        private List<Tag> GetTags(Source source, PrettyScraper scraper)
        {
            if (source.ArticleTagPath.IsNullOrEmpty())
            {
                return new List<Tag>();
            }

            var plainTags = scraper.Parser.Init(source.ArticleTagPath!).TextContent();
            var tagsEntity = new List<Tag>();
            foreach (var plainTag in plainTags)
            {
                var tag = plainTag.Trim(' ', '\n', '\t');
                if (tag.Length >= 20)
                {
                    continue;
                }

                tagsEntity.Add(new Tag { Name = tag });
            }

            return tagsEntity;
        }

        private string? GetBody(Source source, PrettyScraper scraper)
        {
            var body = scraper.Parser
                .Init(source.ArticleBodyCollectionsPath)
                .SelectAll(source.ArticleBodyItemPath)
                .RemoveAll("script")
                .WrapAll("iframe", "div", "ratio ratio-16x9")
                .WrapAll("embed", "div", "ratio ratio-16x9")
                .WrapAll("video", "div", "ratio ratio-16x9")
                .ToHtml();

            return body;
        }
    }
}