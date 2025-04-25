using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Comment?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public GetCommentByIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Comment?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .Include(comment => comment.User)
                .FirstOrDefaultAsync(comment => comment.Id == request.CommentId);
        }
    }
}
