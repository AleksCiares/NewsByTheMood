using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.CommandHandlers
{
    class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public UpdateTopicCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Topics.Update(request.Topic);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
