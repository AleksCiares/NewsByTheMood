using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesCountByTopicQueryHandler : IRequestHandler<GetArticlesCountByTopicQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesCountByTopicQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetArticlesCountByTopicQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= request.Positivity)
                .Where(article => article.Source.TopicId == request.TopicId)
                .CountAsync(cancellationToken);
        }
    }
}
