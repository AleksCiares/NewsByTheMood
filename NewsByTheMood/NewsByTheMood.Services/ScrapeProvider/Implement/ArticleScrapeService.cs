using System.Text.RegularExpressions;
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
        private readonly ITagService _tagService;

        public ArticleScrapeService(IOptions<WebScrapeOptions> options, IArticleService articleService, ITagService tagService)
        {
            _options = options.Value;
            _articleService = articleService;
            _tagService = tagService;
        }

        public async Task LoadArticle(Source source, string articleUrl)
        {
            var scraper = CreateScraper(source);
            await scraper.GetPageAsync(articleUrl);

            var article = await ParseArticleAsync(source, scraper);
            article.Url = articleUrl;

            scraper.Dispose();
            await _articleService.AddAsync(article);
        }

        public async Task LoadArticles(Source source)
        {
            var scraper = CreateScraper(source);

            await scraper.GetPageAsync(source.Url);
            var articlesUrls = scraper.Parser.Init(source.ArticleCollectionsPath).SelectAll(source.ArticleItemPath).SelectAll(source.ArticleUrlPath).GetAttributes("href");

            articlesUrls.Reverse();
            for (var i = articlesUrls.Count - 1; i >= 0; i--)
            {
                if (!Uri.IsWellFormedUriString(articlesUrls[i], uriKind: UriKind.Absolute))
                {
                    articlesUrls[i] = new Uri(new Uri(source.Url), articlesUrls[i]).ToString();
                }

                if (await _articleService.IsExistByUrlAsync(articlesUrls[i]))
                {
                    articlesUrls.RemoveAt(i);
                }
            }

            var articles = new List<Article>();
            foreach (var url in articlesUrls)
            {
                await scraper.GetPageAsync(url);
                var article = await ParseArticleAsync(source, scraper);
                article.Url = url;

                articles.Add(article);
                //await _articleService.AddAsync(article);
            }

            scraper.Dispose();
            await _articleService.AddRangeAsync(articles.ToArray());
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

        private async Task<Article> ParseArticleAsync(Source source, PrettyScraper scraper)
        {
            return new Article()
            {
                Title = scraper.Parser.Init(source.ArticleTitlePath).TextContent().ElementAtOrDefault(0)! ?? "No found title",
                PreviewImgUrl = LoadPreviewImage(source, scraper),
                Body = scraper.Parser.Init(source.ArticleBodyCollectionsPath).SelectAll(source.ArticleBodyItemPath).ToHtml(),
                PublishDate = LoadPublishDateDate(source, scraper),
                Positivity = 0,
                Rating = 0,
                SourceId = source.Id,
                Tags = await LoadTagsAsync(source, scraper)
            };
        }

        private string? LoadPreviewImage(Source source, PrettyScraper scraper)
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
                    var matches = regex.Matches(path!);
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

        private DateTime? LoadPublishDateDate(Source source, PrettyScraper scraper)
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

        private async Task<List<Tag>> LoadTagsAsync(Source source, PrettyScraper scraper)
        {
            if (source.ArticleTagPath.IsNullOrEmpty())
            {
                return new List<Tag>();
            }

            var plainTags = scraper.Parser.Init(source.ArticleTagPath!).TextContent();
            var tagsEntity = new List<Tag>();
            foreach (var plainTag in plainTags)
            {
                //var tag = Regex.Replace(plainTag, @"\s+", "", RegexOptions.Compiled);
                var tag = plainTag.Trim(' ', '\n', '\t');
                if (tag.Length >= 20)
                {
                    continue;
                }

                /*var tagEntity = await _tagService.GetByNameAsync(tag);
                if (tagEntity == null)
                {
                    await _tagService.AddAsync(new Tag { Name = tag });
                    tagEntity = await _tagService.GetByNameAsync(tag);
                }*/

                tagsEntity.Add(new Tag { Name = tag });
            }

            return tagsEntity;
        }
    }
}
