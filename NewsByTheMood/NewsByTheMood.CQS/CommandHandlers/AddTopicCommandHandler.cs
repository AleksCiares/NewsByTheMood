using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddTopicCommandHandler : IRequestHandler<AddTopicCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddTopicCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddTopicCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Topics.AddAsync(request.Topic, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
