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
    public class GetAllTopicsQueryHandler : IRequestHandler<GetAllTopicsQuery, IEnumerable<Topic>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetAllTopicsQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Topic>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }
    }
}
