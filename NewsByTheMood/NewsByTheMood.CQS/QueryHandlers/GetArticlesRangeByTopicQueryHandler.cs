using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetArticlesRangeByTopicQueryHandler : IRequestHandler<GetArticlesRangeByTopicQuery, IEnumerable<Article>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesRangeByTopicQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Article>> Handle(GetArticlesRangeByTopicQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= request.Positivity)
                .Where(article => article.Source.TopicId == request.TopicId)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);
        }
    }
}
