using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public UpdateSourceCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateSourceCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Sources.Update(request.Source);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
