using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class IsExistsSourceByNameQueryHandler : IRequestHandler<IsExistsSourceByNameQuery, bool>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public IsExistsSourceByNameQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(IsExistsSourceByNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .Where(source => source.Name.Equals(request.SourceName))
                .AnyAsync(cancellationToken);
        }
    }
}
