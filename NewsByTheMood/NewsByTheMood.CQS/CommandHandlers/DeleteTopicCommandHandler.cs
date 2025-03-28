using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public DeleteTopicCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Topics.Remove(request.Topic);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
