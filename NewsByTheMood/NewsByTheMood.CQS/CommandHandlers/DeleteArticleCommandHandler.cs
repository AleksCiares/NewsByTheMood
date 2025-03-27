using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public DeleteArticleCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Articles.Remove(request.Article);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
