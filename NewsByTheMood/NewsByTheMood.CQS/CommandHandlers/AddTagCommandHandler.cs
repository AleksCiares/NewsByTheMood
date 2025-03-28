using Azure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddTagCommandHandler : IRequestHandler<AddTagCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddTagCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Tags.AddAsync(request.Tag);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
