using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class DeleteCommentsRangeCommandHandler : IRequestHandler<DeleteCommentsRangeCommand, long[]>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public DeleteCommentsRangeCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long[]> Handle(DeleteCommentsRangeCommand request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .Where(c => request.Ids.Contains(c.Id))
                .ToListAsync(cancellationToken);

            foreach (var comment in comments)
            {
                comment.Deleted = true;
            }

            _dbContext.Comments.UpdateRange(comments);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return comments.Select(comment => comment.Id).ToArray();
        }
    }
}
