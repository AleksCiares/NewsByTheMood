using MediatR;
using Microsoft.Extensions.Logging;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class ArticleService : IArticleService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(IMediator mediator, ILogger<ArticleService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<int> CountAsync(short positivity, CancellationToken cancellationToken = default)
        {
            if (positivity < 0)
            {
                _logger.LogWarning($"Positivity is less than 0. Positivity: {positivity}. Proccess aborted.");
                return 0;
            }

            var count = await _mediator.Send(new GetArticlesCountQuery() 
            {
                Positivity = positivity 
            }, cancellationToken);

            _logger.LogDebug($"{count} articles were found. Positivity: {positivity}");

            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or PageNumber/PageSize is less than or equal to 0. " +
                    $"Positivity: {positivity}, PageNumber: {pageNumber}, PageSize: {pageSize}. Proccess aborted.");
                return Array.Empty<Article>();
            }

            var articles = await _mediator.Send(new GetLatestArticlesRangeQuery() 
            { 
                Positivity = positivity, 
                Page = pageNumber, 
                PageSize = pageSize }
            ,cancellationToken);

            _logger.LogDebug($"{articles.Count()} articles were found. Positivity: {positivity}, PageNumber: {pageNumber}, " +
                    $"PageSize {pageSize}");

            return articles;
        }

        public async Task<int> CountByTopicAsync(short positivity, long topicId, 
            CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || topicId <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or TopicId is less than or equal to 0. " +
                    $"Positivity: {positivity}, TopicId: {topicId}. Proccess aborted.");
                return 0;
            }

            var count = await _mediator.Send(new GetArticlesCountByTopicQuery() 
            { 
                Positivity = positivity, 
                TopicId = topicId 
            }, 
            cancellationToken);

            _logger.LogDebug($"{count} articles were found. Positivity: {positivity}, TopicId: {topicId}");

            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeByTopicAsync(short positivity, long topicId, int pageNumber, 
            int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || topicId <= 0 || pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or TopicId/PageNumber/PageSize is less than or equal to 0. " +
                    $"Positivity: {positivity}, TopicId: {topicId}, PageNumber: {pageNumber}, PageSize: {pageSize}. Process aborted.");
                return Array.Empty<Article>();
            }

            var articles = await _mediator.Send(new GetArticlesRangeByTopicQuery() 
            { 
                Positivity = positivity, 
                TopicId = topicId, 
                Page = pageNumber, 
                PageSize = pageSize 
            }, 
            cancellationToken);

            _logger.LogDebug($"{articles.Count()} articles were found. Positivity: {positivity}, TopicId: {topicId}, " +
                $"PageNumber: {pageNumber}, PageSize: {pageSize}");

            return articles;
        }

        public async Task<int> CountFavoriteAsync(short positivity, IEnumerable<long> topicsIds, 
            CancellationToken cancellationToken = default)
        {
            if (positivity < 0)
            {
                _logger.LogWarning($"Positivity is less than 0. Positivity: {positivity}. Process aborted.");
                return 0;
            }

            var count = await _mediator.Send(new GetArticlesCountByFavoriteQuery() 
            { 
                Positivity = positivity, 
                TopicIds = topicsIds 
            },
            cancellationToken);

            _logger.LogDebug($"{count} articles were found. Positivity: {positivity}, TopicsIds: {string.Join(", ", topicsIds)}");

            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeFavoriteAsync(short positivity, IEnumerable<long> topicsIds, 
            int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or TopicId/PageNumber/PageSize is less than or equal to 0. " +
                    $"Positivity: {positivity}, PageNumber: {pageNumber}, PageSize: {pageSize}. Proccess aborted.");
                return Array.Empty<Article>();
            }

            var articles = await _mediator.Send(new GetArticlesRangeByFavoriteQuery() 
            { 
                Page = pageNumber,
                PageSize = pageSize,
                TopicIds = topicsIds,
                Positivity = positivity
            },
            cancellationToken);

            _logger.LogDebug($"{articles.Count()} articles were fetch. Positivity: {positivity}, " +
                $"TopicsIds: {string.Join(", ", topicsIds)}, PageNumber: {pageNumber}, PageSize: {pageSize}");

            return articles;
        }

        public async Task<Article?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Article id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return null;
            }

            var article = await _mediator.Send(new GetArticleByIdQuery() 
            { 
                Id = id 
            }, cancellationToken);

            if (article != null)
            {
                _logger.LogDebug($"Article was fetched successfully. ArticleId: {id}");
            }
            else
            {
                _logger.LogWarning($"No article was found. ArticleId: {id}");
            }

            return article;
        }

        public async Task<bool> IsExistsByUrlAsync(string articleUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(articleUrl))
            {
                _logger.LogWarning($"Article url is null or empty. ArticleUrl: {articleUrl}. Proccess aborted.");
                return false;
            }

            var isExists = await _mediator.Send(new IsExistsArticleByUrlQuery() 
            { 
                ArticleUrl = articleUrl 
            }, cancellationToken);

            _logger.LogDebug($"Article with url {articleUrl} exists: {isExists}");

            return isExists;
        }

        public async Task<bool> IsExistsByIdAsync(long articleId, CancellationToken cancellationToken = default)
        {
            if (articleId <= 0)
            {
                _logger.LogWarning($"ArticleId is less than or equal to 0. ArticleId: {articleId}. Proccess aborted.");
                return false;
            }

            var isExists = await _mediator.Send(new IsExistsArticleByIdQuery() { Id = articleId }, cancellationToken);

           _logger.LogDebug($"Article with id {articleId} exists: {isExists}");

            return isExists;
        }

        public async Task<bool> AddAsync(Article article, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding article... ArticleUrl: {article.Url}.");

            if (await IsExistsByUrlAsync(article.Url))
            {
                _logger.LogWarning($"Article with url {article.Url} already exists. Proccess aborted.");
                return false;
            }

            // TODO: Check if article is created successfully
            await _mediator.Send(new AddArticleCommand() { Article = article }, cancellationToken);

            _logger.LogInformation($"Article was added successfully. ArticleUrl: {article.Url}.");

            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Article> articles, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding {articles.Count()} articles... ArticlesUrls:{Environment.NewLine}" +
                 $"{string.Join(Environment.NewLine, articles.Select(article => article.Url))}");

            if (articles == null || !articles.Any())
            {
                _logger.LogWarning($"Articles collection is null or empty. Proccess aborted.");
                return false;
            }

            var existinsUrls = new List<string>();
            foreach (var article in articles)
            {
                if (await IsExistsByUrlAsync(article.Url, cancellationToken))
                {
                   existinsUrls.Add(article.Url);
                }
            }

            if(existinsUrls.Count > 0)
            {
                _logger.LogWarning($"The following articles already exist:{Environment.NewLine}" +
                    $"{string.Join(Environment.NewLine, existinsUrls)}");
            }

            var articlesToAdd = articles.Where(article => !existinsUrls.Contains(article.Url)).ToList();

            if (!articlesToAdd.Any())
            {
                _logger.LogWarning("No new articles to add. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new AddArticlesRangeCommand() 
            { 
                Articles = articles 
            }, cancellationToken);

            _logger.LogInformation($"{articlesToAdd.Count} articles were added successfully:{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, articlesToAdd.Select(article => article.Url))}");

            return true;
        }

        public async Task<bool> UpdateAsync(Article article, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating article... ArticleId: {article.Id}.");

            var articleEntity = await GetByIdAsync(article.Id, cancellationToken);

            if (articleEntity == null)
            {
                _logger.LogWarning($"Article not found. ArticleId: {article.Id}. Proccess aborted.");
                return false;
            }

            if (await IsExistsByUrlAsync(article.Url, cancellationToken) && !article.Url.Equals(articleEntity.Url))
            {
                _logger.LogWarning($"Article with same url already exists. ArticleUrl: {article.Url}. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new UpdateArticleCommand() 
            { 
                Article = article 
            }, cancellationToken);

            _logger.LogInformation($"Article was updated successfully. ArticleId: {article.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting article... ArticleId: {id}.");

            var article = await GetByIdAsync(id, cancellationToken);

            if (article == null)
            {
                _logger.LogWarning($"Article not found. ArticleId: {id}. Procces aborted.");
                return false;
            }

            await _mediator.Send(new DeleteArticleCommand() 
            { 
                Article = article 
            }, cancellationToken);

            _logger.LogInformation($"Article was deleted successfully. ArticleId: {id}");

            return true;
        }

        public async Task<long[]> DeleteRangeAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting {ids.Length} articles... ArticlesIds:{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, ids)}");

            if (ids.Length == 0)
            {
                _logger.LogWarning($"ArticlesIds collection is empty. Proccess aborted.");
                return Array.Empty<long>();
            }

            var result = new List<long>();
            foreach (var id in ids)
            {
                var article = await GetByIdAsync(id, cancellationToken);

                if (article != null)
                {
                    await _mediator.Send(new DeleteArticleCommand() 
                    { 
                        Article = article 
                    }, cancellationToken);

                    _logger.LogInformation($"Article was deleted successfully. ArticleId: {id}");
                    result.Add(id);
                }
                else
                {
                    _logger.LogWarning($"Article not found. ArticleId: {id}.");
                }
              
            }

            _logger.LogInformation($"{result.Count} articles were deleted successfully.");

            return result.ToArray();
        }
    }
}
