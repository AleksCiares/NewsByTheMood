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

        private async Task<int> CountAsync(
            short positivity,
            bool ignoreActivity,
            IEnumerable<long>? topicIds = null,
            CancellationToken cancellationToken = default)
        {
            if (positivity < 0)
            {
                _logger.LogWarning($"Positivity is less than 0. Positivity: {positivity}. Proccess aborted.");
                return 0;
            }

            var count = await _mediator.Send(new GetArticlesCountQuery()
            {
                Positivity = positivity,
                IgnoreActivity = ignoreActivity,
                TopicIds = topicIds
            },
            cancellationToken);

            _logger.LogDebug($"{count} articles were found. {{ " +
                $"Positivity: {positivity}; " +
                $"IgnoreActivity: {ignoreActivity}; " +
                $"Topic Ids: {{ {string.Join(", ", topicIds ?? Array.Empty<long>())} }}; }}");

            return count;
        }

        private async Task<IEnumerable<Article>> GetRangeAsync(
            int page,
            int pageSize,
            short positivity,
            bool ignoreActivity,
            IEnumerable<long>? topicIds = null,
            CancellationToken cancellationToken = default)
        {
            var isValid = true;

            if (page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Page/PageSize is less than or equal to 0. " +
                    $"Page: {page}, PageSize: {pageSize}. Proccess will be aborted.");
                isValid = false;
            }

            if (positivity < 0)
            {
                _logger.LogWarning($"Positivity is less than 0. Positivity: {positivity}. Proccess will be aborted.");
                isValid = false;
            }

            if (!isValid)
            {
                _logger.LogWarning($"Proccess aborted.");
                return Array.Empty<Article>();
            }

            var articles = await _mediator.Send(new GetArticlesRangeQuery()
            {
                Page = page,
                PageSize = pageSize,
                Positivity = positivity,
                IgnoreActivity = ignoreActivity,
                TopicIds = topicIds
            },
            cancellationToken);

            _logger.LogDebug($"{articles.Count()} articles were fetch successfully. {{ " +
                $"Page: {page}; " +
                $"PageSize: {pageSize}; " +
                $"Positivity: {positivity}; " +
                $"IgnoreActivity: {ignoreActivity}; " +
                $"Topic Ids: {{ {string.Join(", ", topicIds ?? Array.Empty<long>())} }}; }}");

            return articles;
        }

        public async Task<int> CountLatestAsync(short positivity, bool ignoreActivity = false, 
            CancellationToken cancellationToken = default)
        {
            var count = await CountAsync(
                positivity: positivity, 
                ignoreActivity: ignoreActivity, 
                cancellationToken: cancellationToken);
            
            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize,
            bool ignoreActivity = false, CancellationToken cancellationToken = default)
        {

            var articles = await GetRangeAsync(
                page: pageNumber,
                pageSize: pageSize,
                positivity: positivity,
                ignoreActivity: ignoreActivity,
                cancellationToken: cancellationToken);

            return articles;
        }

        public async Task<int> CountByTopicAsync(short positivity, long topicId, bool ignoreActivity = false, 
            CancellationToken cancellationToken = default)
        {
            if (topicId <= 0)
            {
                _logger.LogWarning($"TopicId is less than or equal to 0. TopicId: {topicId}. Proccess aborted.");
                return 0;
            }

            var count = await CountAsync(
                positivity: positivity,
                ignoreActivity: ignoreActivity,
                topicIds: new[] { topicId },
                cancellationToken: cancellationToken);

            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeByTopicAsync(short positivity, long topicId, int pageNumber, 
            int pageSize, bool ignoreActivity = false, CancellationToken cancellationToken = default)
        {
            if (topicId <= 0 )
            {
                _logger.LogWarning($"TopicId is less than or equal to 0. TopicId: {topicId}. Process aborted.");
                return Array.Empty<Article>();
            }

            var articles = await GetRangeAsync(
                page: pageNumber,
                pageSize: pageSize,
                positivity: positivity,
                ignoreActivity: ignoreActivity,
                topicIds: new[] { topicId },
                cancellationToken: cancellationToken);

            return articles;
        }

        public async Task<int> CountFavoriteAsync(short positivity, IEnumerable<long> topicIds,
            bool ignoreActivity = false, CancellationToken cancellationToken = default)
        {
            if (topicIds.Count() == 0)
            {
                return 0;
            }

            if (topicIds.Any(topicIds => topicIds <= 0))
            {
                _logger.LogWarning($"Some topic`s id is less than or equal to 0. " +
                    $"Topic ids: {{ {string.Join(", ", topicIds)} }}. " +
                    $"Process aborted.");
                return 0;
            }

            var count = await CountAsync(
                positivity: positivity,
                ignoreActivity: ignoreActivity,
                topicIds: topicIds,
                cancellationToken: cancellationToken);

            return count;
        }

        public async Task<IEnumerable<Article>> GetRangeFavoriteAsync(short positivity, IEnumerable<long> topicIds, 
            int pageNumber, int pageSize, bool ignoreActivity = false, CancellationToken cancellationToken = default)
        {
            if (topicIds.Count() == 0)
            {
                return Array.Empty<Article>();
            }

            if (topicIds.Any(topicIds => topicIds <= 0))
            {
                _logger.LogWarning($"Some topic`s id is less than or equal to 0. " +
                    $"Topic ids: {{ {string.Join(", ", topicIds)} }}. " +
                    $"Process aborted.");
                return Array.Empty<Article>();
            }

            var articles = await GetRangeAsync(
                page: pageNumber,
                pageSize: pageSize,
                positivity: positivity,
                ignoreActivity: ignoreActivity,
                topicIds: topicIds,
                cancellationToken: cancellationToken);

            return articles;
        }

        public async Task<Article?> GetByIdAsync(long id, bool ignoreActivity = false, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Article id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return null;
            }

            var article = await _mediator.Send(new GetArticleByIdQuery() 
            { 
                Id = id 
            }, 
            cancellationToken);

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
            }, 
            cancellationToken);

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

            var isExists = await _mediator.Send(new IsExistsArticleByIdQuery() 
            { 
                Id = articleId 
            }, 
            cancellationToken);

           _logger.LogDebug($"Article with id {articleId} exists: {isExists}");

            return isExists;
        }

        public async Task<bool> AddAsync(Article article, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding article... ArticleUrl: {article.Url}.");

            if (await IsExistsByUrlAsync(article.Url, cancellationToken))
            {
                _logger.LogError($"Article with url {article.Url} already exists. Proccess aborted.");
                return false;
            }

            // TODO: Check if article is created successfully
            await _mediator.Send(new AddArticleCommand() 
            { 
                Article = article 
            }, 
            cancellationToken);

            _logger.LogInformation($"Article was added successfully. ArticleUrl: {article.Url}.");

            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Article> articles, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding {articles.Count()} articles... ArticlesUrls:{Environment.NewLine}" +
                 $"{string.Join(Environment.NewLine, articles.Select(article => article.Url))}");

            if (articles == null || !articles.Any())
            {
                _logger.LogError($"Articles collection is null or empty. Proccess aborted.");
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
                _logger.LogError("No new articles to add. Proccess aborted.");
                return false;
            }

            // TODO: Check if article is created successfully
            await _mediator.Send(new AddArticlesRangeCommand() 
            { 
                Articles = articles 
            }, 
            cancellationToken);

            _logger.LogInformation($"{articlesToAdd.Count} articles were added successfully:{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, articlesToAdd.Select(article => article.Url))}");

            return true;
        }

        public async Task<bool> UpdateAsync(Article article, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating article... ArticleId: {article.Id}.");

            var articleEntity = await GetByIdAsync(article.Id, true, cancellationToken);

            if (articleEntity == null)
            {
                _logger.LogError($"Article not found. ArticleId: {article.Id}. Proccess aborted.");
                return false;
            }

            if (await IsExistsByUrlAsync(article.Url, cancellationToken) && !article.Url.Equals(articleEntity.Url))
            {
                _logger.LogError($"Article with same url already exists. ArticleUrl: {article.Url}. Proccess aborted.");
                return false;
            }

            // TODO: Check if article is updated successfully
            await _mediator.Send(new UpdateArticleCommand() 
            { 
                Article = article 
            }, 
            cancellationToken);

            _logger.LogInformation($"Article was updated successfully. ArticleId: {article.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting article... ArticleId: {id}.");

            if (id <= 0)
            {
                _logger.LogError($"Article id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return false;
            }

            var isExists = await IsExistsByIdAsync(id, cancellationToken);

            if (!isExists)
            {
                _logger.LogError($"Article not found. ArticleId: {id}. Procces aborted.");
                return false;
            }

            // TODO: Check if article is deleted successfully
            await _mediator.Send(new DeleteArticleCommand() 
            { 
                ArticleId = id
            }, 
            cancellationToken);

            _logger.LogInformation($"Article was deleted successfully. ArticleId: {id}");

            return true;
        }

        public async Task<long[]> DeleteRangeAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting {ids.Length} articles... ArticlesIds: {{ " +
                $"{string.Join(", ", ids)} }}.");

            if (ids.Length == 0)
            {
                _logger.LogError($"ArticlesIds collection is empty. Proccess aborted.");
                return Array.Empty<long>();
            }

            var result = new List<long>();
            foreach (var id in ids)
            {
                var isExists = await IsExistsByIdAsync(id, cancellationToken);
                if (isExists)
                {
                    await _mediator.Send(new DeleteArticleCommand() 
                    { 
                        ArticleId = id
                    },
                    cancellationToken);

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
