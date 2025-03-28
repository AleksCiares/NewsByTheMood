using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetSourcesRangeQueryHandler : IRequestHandler<GetSourcesRangeQuery, IEnumerable<Source>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetSourcesRangeQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Source>> Handle(GetSourcesRangeQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .Include(source => source.Topic)
                .Include(source => source.Articles)
                .OrderByDescending(source => source.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);
        }
    }
}
