using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataLoadProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.WebScrapeProvider.Abstract;
using NewsByTheMood.Services.WebScrapeProvider.Implement;
using System.Text.RegularExpressions;
using WebScraper.Core.Settings;

namespace NewsByTheMood.Services.DataLoadProvider.Implement
{
    public class ArticleLoadService : IArticleLoadService
    {
        private readonly WebScrapeOptions _options;
        private readonly IArticleService _articleService;
        private readonly ITagService _tagService;

        public ArticleLoadService(IOptions<WebScrapeOptions> options, IArticleService articleService, ITagService tagService)
        {
            _options = options.Value;
            _articleService = articleService;
            _tagService = tagService;
        }

        public async Task LoadArticles(Source source)
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

            var loaderSettings = new WebLoaderSettings
            {
                UserAgent = userAgent,
                ProxySettings = proxy,
                AcceptInsecureCertificates = source.AcceptInsecureCerts,
                PageElementLoaded = source.PageElementLoaded,
                PageLoadTimeout = TimeSpan.FromSeconds(source.PageLoadTimeout),
                ElementLoadTimeout = TimeSpan.FromSeconds(source.ElementLoadTimeout)
            };
            IArticleScrapeService scraper;
            if (source.HasDynamicPage)
            {
                scraper = new DynamicArticleScrapeService(loaderSettings);
            }
            else
            {
                scraper = new StaticArticleScrapeService(loaderSettings);
            }

            var articlesUrls = await scraper.GetArticlesUrlsAsync(source);
            foreach (var url in articlesUrls)
            {
                if (await _articleService.IsExistByUrl(url))
                {
                    articlesUrls.Remove(url);
                }
            }

            var articles = await scraper.GetArticlesAsync(source, articlesUrls.ToArray());
            var tags = new List<Tag>();
            foreach (var article in articles)
            {
                await _articleService.AddAsync(new Article()
                {
                    Url = article.Url,
                    Title = article.Title,
                    PreviewImgUrl = article.PreviewImgUrl,
                    Body = article.Body,
                    PublishDate = this.TryParseDate(article.PublishDate),
                    Positivity = 10,
                    Rating = 0,
                    SourceId = source.Id,
                    Tags = article.Tags != null ? await this.SaveTagsAsync(article.Tags) : new List<Tag>()
                });
            }
        }

        private async Task<List<Tag>> SaveTagsAsync(string[] tags)
        {
            var tagsList = new List<Tag>();
            foreach (var tag in tags)
            {
                var whiteSpaceLessTag = Regex.Replace(tag, @"\s+", "");
                if (!await _tagService.IsExistsAsync(whiteSpaceLessTag))
                {
                    await _tagService.AddAsync(new Tag { Name = whiteSpaceLessTag });
                }

                tagsList.Add(new Tag { Name = whiteSpaceLessTag });

                //var tagEntity = await _tagService.GetByName(whiteSpaceLessTag);
                //if (tagEntity != null)
                //{
                //    tagsList.Add(tagEntity);
                //}
            }

            return tagsList;
        }

        private DateTime? TryParseDate(string? publishDate)
        {
            if (DateTime.TryParse(publishDate, out var date))
            {
                return date;
            }
            return null;
        }
    }
}
