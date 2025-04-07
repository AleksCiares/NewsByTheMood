using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class DeleteSourceCommandHandler : IRequestHandler<DeleteSourceCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public DeleteSourceCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Удаление всех связанных статей
                var articles = await _dbContext.Articles
                    .Include(article => article.Tags)
                    .Where(a => a.SourceId == request.Source.Id)
                    .ToListAsync(cancellationToken);

                _dbContext.Articles.RemoveRange(articles);

                // Удаление источника
                _dbContext.Sources.Remove(request.Source);

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
