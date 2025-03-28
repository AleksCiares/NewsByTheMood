using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

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
            _dbContext.Sources.Remove(request.Source);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
