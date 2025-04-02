using MediatR;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class ArticleService : IArticleService
    {
        /*private readonly NewsByTheMoodDbContext _dbContext;*/
        private readonly IMediator _mediator;

        public ArticleService(/*NewsByTheMoodDbContext dbContext, */IMediator mediator)
        {
            /*_dbContext = dbContext;*/
            _mediator = mediator;
        }

        public async Task<int> CountAsync(short positivity, CancellationToken cancellationToken = default)
        {
            if (positivity < 0)
            {
                return 0;
            }

            return await _mediator.Send(new GetArticlesCountQuery() { Positivity = positivity }, cancellationToken);
        }

        public async Task<IEnumerable<Article>> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || pageNumber <= 0 || pageSize <= 0)
            {
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
                return 0;
            }

            return await _mediator.Send(new GetArticlesCountByTopicQuery() { Positivity = positivity, TopicId = topicId }, cancellationToken);
        }

        public async Task<IEnumerable<Article>> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (positivity < 0 || topicId <= 0 || pageNumber <= 0 || pageSize <= 0)
            {
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
                return null;
            }

           return await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<bool> IsExistsByUrlAsync(string articleUrl, CancellationToken cancellationToken = default)
        {
            if (articleUrl.IsNullOrEmpty())
            {
                return false;
            }

            return await _mediator.Send(new IsExistsArticleByUrlQuery() { ArticleUrl = articleUrl }, cancellationToken);
        }

        public async Task<bool> AddAsync(Article article, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new AddArticleCommand() { Article = article }, cancellationToken);
            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Article> articles, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new AddArticlesRangeCommand() { Articles = articles }, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(Article article, CancellationToken cancellationToken = default)
        {
            var articleEntity = await _mediator.Send(new GetArticleByIdQuery() { Id = article.Id }, cancellationToken);
            if (articleEntity == null)
            {
                return false;
            }
            await _mediator.Send(new UpdateArticleCommand() { Article = article }, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var article = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
            if (article == null)
            {
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
                var article = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);
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
