using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var article = await _dbContext.Articles
                .SingleAsync(a => a.Id == request.ArticleId, cancellationToken);

            _dbContext.Articles.Remove(article);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var orphanedTags = _dbContext.Tags
                .Where(tag => !tag.Articles.Any())
                .ToList();
            _dbContext.Tags.RemoveRange(orphanedTags);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
