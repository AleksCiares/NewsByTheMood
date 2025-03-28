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
    public class GetSourcesCountQueryHandler : IRequestHandler<GetSourcesCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetSourcesCountQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetSourcesCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }
    }
}
