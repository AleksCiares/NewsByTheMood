using MediatR;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.CommandHandlers
{
    public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public AddSourceCommandHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddSourceCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Sources.AddAsync(request.Source);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
