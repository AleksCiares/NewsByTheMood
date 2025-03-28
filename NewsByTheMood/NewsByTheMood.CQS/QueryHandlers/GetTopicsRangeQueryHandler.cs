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
    public class GetTopicsRangeQueryHandler : IRequestHandler<GetTopicsRangeQuery, IEnumerable<Topic>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetTopicsRangeQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Topic>> Handle(GetTopicsRangeQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .OrderByDescending(topic => topic.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);
        }
    }
}
