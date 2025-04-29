using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetCommentsCountByArticleIdQueryHandler : IRequestHandler<GetCommentsCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetCommentsCountByArticleIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public async Task<int> Handle(GetCommentsCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Comments
                 .AsNoTracking()
                 .Where(comment => comment.ArticleId == request.ArticleId)
                 .CountAsync(cancellationToken);
        }
    }
}
