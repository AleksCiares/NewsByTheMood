using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public UpdateArticleCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            this._dbContext.Articles.Update(request.Article);
            await this._dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
