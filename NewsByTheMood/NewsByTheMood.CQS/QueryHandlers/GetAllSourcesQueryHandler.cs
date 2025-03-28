﻿using MediatR;
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
    public class GetAllSourcesQueryHandler : IRequestHandler<GetAllSourcesQuery, IEnumerable<Source>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetAllSourcesQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Source>> Handle(GetAllSourcesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
        }
    }
}
