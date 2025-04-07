using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
                _logger.LogWarning($"Positivity is less than 0. Positivity: {positivity}");
                return 0;
            }

            return await _mediator.Send(new GetArticlesCountQuery() { Positivity = positivity }, cancellationToken);
        }

        public async Task<IEnumerable<Article>> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or pageNumber/pageSize is less than or equal to 0. Positivity: " +
                    $"{positivity}, PageNumber: {pageNumber}, PageSize: {pageSize}");
                return Array.Empty<Article>();
            }

            return await _mediator.Send(new GetLatestArticlesRangeQuery() 
            { 
                Positivity = positivity, 
                Page = pageNumber, 
                PageSize = pageSize }
            ,cancellationToken);
        }

        public async Task<int> CountByTopicAsync(short positivity, Int64 topicId, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || topicId <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or topicId is less than or equal to 0. Positivity: {positivity}, TopicId: {topicId}");
                return 0;
            }

            return await _mediator.Send(new GetArticlesCountByTopicQuery() { Positivity = positivity, TopicId = topicId }, cancellationToken);
        }

        public async Task<IEnumerable<Article>> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || topicId <= 0 || pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Positivity is less than 0 or topicId is less than or equal to 0 or pageNumber/pageSize is less than or equal to 0. Positivity: " +
                    $"{positivity}, TopicId: {topicId}, PageNumber: {pageNumber}, PageSize: {pageSize}");
                return Array.Empty<Article>();
            }

            return await _mediator.Send(new GetArticlesRangeByTopicQuery() 
            { 
                Positivity = positivity, 
                TopicId = topicId, Page = pageNumber, 
                PageSize = pageSize 
            }, 
            cancellationToken);
        }

        public async Task<Article?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Article id is less than or equal to 0. Id: {id}");
                return null;
            }

           return await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<bool> IsExistsByUrlAsync(string articleUrl, CancellationToken cancellationToken = default)
        {
            if (articleUrl.IsNullOrEmpty())
            {
                _logger.LogWarning($"Article url is null or empty. ArticleUrl: {articleUrl}");
                return false;
            }

            return await _mediator.Send(new IsExistsArticleByUrlQuery() { ArticleUrl = articleUrl }, cancellationToken);
        }

        public async Task<bool> AddAsync(Article article, CancellationToken cancellationToken = default)
        {
            if(await IsExistsByUrlAsync(article.Url))
            {
                _logger.LogWarning($"Article with url {article.Url} already exists.");
                return false;
            }

            await _mediator.Send(new AddArticleCommand() { Article = article }, cancellationToken);
            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Article> articles, CancellationToken cancellationToken = default)
        {
            if (articles == null || !articles.Any())
            {
                _logger.LogWarning($"Articles collection is null or empty.");
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
                _logger.LogWarning($"Articles with urls {string.Join(", ", existinsUrls)} already exists.");
            }

            var articlesToAdd = articles.Where(article => !existinsUrls.Contains(article.Url)).ToList();

            if (!articlesToAdd.Any())
            {
                _logger.LogWarning("No new articles to add.");
                return false;
            }

            await _mediator.Send(new AddArticlesRangeCommand() { Articles = articles }, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(Article article, CancellationToken cancellationToken = default)
        {
            var articleEntity = await GetByIdAsync(article.Id, cancellationToken);
            if (articleEntity == null)
            {
                _logger.LogWarning($"Article with id {article.Id} not found.");
                return false;
            }
            if (await IsExistsByUrlAsync(article.Url, cancellationToken) && !article.Url.Equals(articleEntity.Url))
            {
                _logger.LogWarning($"Article with url {article.Url} already exists.");
                return false;
            }

            await _mediator.Send(new UpdateArticleCommand() { Article = article }, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var article = await GetByIdAsync(id, cancellationToken);
            if (article == null)
            {
                _logger.LogWarning($"Article with id {id} not found.");
                return false;
            }

            await _mediator.Send(new DeleteArticleCommand() { Article = article }, cancellationToken);
            return true;
        }

        public async Task<long[]> DeleteRangeAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            var result = new List<long>();

            foreach (var id in ids)
            {
                var article = await GetByIdAsync(id, cancellationToken);
                if (article != null)
                {
                    await _mediator.Send(new DeleteArticleCommand() { Article = article }, cancellationToken);
                    result.Add(id);
                }
                
            }
          
            return result.ToArray();
        }
    }


}
