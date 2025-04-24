
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetCommentsRangeByArticleIdQueryHandler : IRequestHandler<GetCommentsRangeByArticleIdQuery, IEnumerable<Comment>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetCommentsRangeByArticleIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Comment>> Handle(GetCommentsRangeByArticleIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(comment => comment.ArticleId == request.ArticleId)
                .Include(comment => comment.User)
                .OrderBy(comment => comment.PublishDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync();
        }
    }
}
