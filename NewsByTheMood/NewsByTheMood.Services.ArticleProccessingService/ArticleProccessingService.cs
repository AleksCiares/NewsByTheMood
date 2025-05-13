using Microsoft.Extensions.Logging;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.PositifityRater.Abstract;
using NewsByTheMood.Services.PositifityRater.Implement;
using NewsByTheMood.Services.ScrapeProvider.Abstract;

namespace NewsByTheMood.Services.ArticleProccessingService
{
    public class ArticleProccessingService
    {
        private readonly ISourceService _sourceService;
        private readonly IArticleService _articleService;
        private readonly IArticleScrapeService _articleScrapeService;
        private readonly ILogger<ArticleProccessingService> _logger;

        public ArticleProccessingService(
            ISourceService sourceService,
            IArticleService articleService,
            IArticleScrapeService articleScrapeService,
            ILogger<ArticleProccessingService> logger)
        {
            _sourceService = sourceService;
            _articleService = articleService;
            _articleScrapeService = articleScrapeService;
            _logger = logger;
        }

        public async Task<bool> ProcessArticlesBySourceAsync(long sourceId)
        {
            try
            {
                var source = await _sourceService.GetByIdAsync(sourceId);
                if (source == null)
                {
                    _logger.LogWarning($"Source with ID {sourceId} not found.");
                    return false;
                }

                var articles = new List<Article>(await _articleScrapeService.ScrapeLatestBySourceAsync(source));

                for(int i = articles.Count - 1; i >= 0; i--)
                {
                    if(await _articleService.IsExistsByUrlAsync(articles[i].Url))
                    {
                        _logger.LogInformation($"Article with URL {articles[i].Url} already exists.");
                        articles.RemoveAt(i);
                        continue;
                    }

                    if (articles[i].Body != null)
                    {
                        articles[i].Positivity = RussianPositivityService.GetPositivity(articles[i].Body);
                    }
                }

                if (articles.Count == 0)
                {
                    _logger.LogInformation($"No new articles found for source ID {sourceId}.");
                    return false;
                }

                var result = await _articleService.AddRangeAsync(articles);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing articles for source ID {sourceId}: {ex.Message}");
                return false;
            }
        }

        public bool ProcessArticlesBySource(long sourceId)
        {
            var result = ProcessArticlesBySourceAsync(sourceId).Result;
            return result;
        }

        public async Task<bool> ProccessCertainArticleAsync(long articleId)
        {
            try
            {
                var existingArticle = await _articleService.GetByIdAsync(articleId);
                if (existingArticle == null)
                {
                    _logger.LogWarning($"Article with ID {articleId} not found.");
                    return false;
                }

                var source = await _sourceService.GetByIdAsync(existingArticle.SourceId);
                if (source == null)
                {
                    _logger.LogWarning($"Source with ID {existingArticle.SourceId} not found.");
                    return false;
                }

                var article = await _articleScrapeService.ScrapeAsync(source, existingArticle.Url);
                if (article == null)
                {
                    _logger.LogWarning($"Failed to scrape article with URL {existingArticle.Url}.");
                    return false;
                }

                article.Id = existingArticle.Id;
                article.SourceId = existingArticle.SourceId;

                var result = await _articleService.UpdateAsync(article);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing article ID {articleId}: {ex.Message}");
                return false;
            }
        }
    }
}
