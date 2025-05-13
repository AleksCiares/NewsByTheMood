using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, long>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddSourceCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Handle(AddSourceCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Sources.AddAsync(request.Source);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return request.Source.Id;
        }
    }
}
