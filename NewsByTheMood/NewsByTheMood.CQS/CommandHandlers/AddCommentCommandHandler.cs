using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddCommentCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                Text = request.Text,
                Position = 0,
                PublishDate = DateTime.UtcNow,
                ArticleId = request.ArticleId,
                UserId = request.UserId
            };

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
