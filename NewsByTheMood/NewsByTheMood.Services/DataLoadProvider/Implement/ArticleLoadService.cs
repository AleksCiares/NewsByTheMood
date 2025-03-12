using Microsoft.Extensions.Options;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataLoadProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.WebScrapeProvider.Abstract;
using NewsByTheMood.Services.WebScrapeProvider.Implement;

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
            var scraper = CreateScraper(source);

            var articlesUrls = await scraper.GetArticlesUrlsAsync(source);
            articlesUrls.Reverse();
            for (var i = articlesUrls.Count - 1; i >= 0; i--)
            {
                if (await _articleService.IsExistByUrlAsync(articlesUrls[i]))
                {
                    articlesUrls.RemoveAt(i);
                }
            }

            var articles = await scraper.GetArticlesAsync(source, articlesUrls.ToArray());
            foreach (var article in articles)
            {
                await _articleService.AddAsync(new Article()
                {
                    Url = article.Url,
                    Title = article.Title,
                    PreviewImgUrl = article.PreviewImgUrl,
                    Body = article.Body,
                    PublishDate = article.PublishDate,
                    Positivity = 0,
                    Rating = 0,
                    SourceId = source.Id,
                    Tags = await SaveTagsAsync(article.Tags)
                });
            }
        }

        public async Task LoadArticle(Source source, string articleUrl)
        {
            var scraper = CreateScraper(source);
            var article = await scraper.GetArticleAsync(source, articleUrl);
            await _articleService.AddAsync(new Article()
            {
                Url = article.Url,
                Title = article.Title,
                PreviewImgUrl = article.PreviewImgUrl,
                Body = article.Body,
                PublishDate = article.PublishDate,
                Positivity = 0,
                Rating = 0,
                SourceId = source.Id,
                Tags = await SaveTagsAsync(article.Tags) 
            });
        }

        private IArticleScrapeService CreateScraper(Source source)
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

            return scraper;
        }

        private async Task<List<Tag>> SaveTagsAsync(string[]? tags)
        {
            if (tags == null)
            {
                return new List<Tag>();
            }

            var tagsList = new List<Tag>();
            foreach (var tag in tags)
            {
                var tagEntity = await _tagService.GetByNameAsync(tag);
                if (tagEntity == null)
                {
                    tagEntity = await _tagService.AddAsync(new Tag { Name = tag });
                }

                tagsList.Add(tagEntity);
            }

            return tagsList;
        }

       
    }
}
