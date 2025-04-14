using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesCountByFavoriteQueryHandler : IRequestHandler<GetArticlesCountByFavoriteQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public GetArticlesCountByFavoriteQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetArticlesCountByFavoriteQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity == request.Positivity &&
                    article.Source.Topic != null &&
                    request.TopicIds.Contains(article.Source.TopicId))
                .CountAsync(cancellationToken);
        }
    }
}
