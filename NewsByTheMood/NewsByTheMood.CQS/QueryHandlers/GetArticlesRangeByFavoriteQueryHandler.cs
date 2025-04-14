using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesRangeByFavoriteQueryHandler : IRequestHandler<GetArticlesRangeByFavoriteQuery, IEnumerable<Article>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesRangeByFavoriteQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Article>> Handle(GetArticlesRangeByFavoriteQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity == request.Positivity &&
                    article.Source.Topic != null &&
                    request.TopicIds.Contains(article.Source.TopicId))
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
