using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesRangeQueryHandler : IRequestHandler<GetArticlesRangeQuery, IEnumerable<Article>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesRangeQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Article>> Handle(GetArticlesRangeQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= request.Positivity);

            if (!request.IgnoreActivity) 
            {
                query = query.Where(article => article.IsActive);
            }

            if (request.TopicIds != null && request.TopicIds.Count() > 0)
            {
                if (request.TopicIds.Count() == 1)
                {
                    query = query.Where(article => article.Source.TopicId == request.TopicIds.ElementAt(0));
                }
                else
                {
                    query = query.Where(article => request.TopicIds.Contains(article.Source.TopicId));
                }
            }

            return await query
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

        }
    }
}
