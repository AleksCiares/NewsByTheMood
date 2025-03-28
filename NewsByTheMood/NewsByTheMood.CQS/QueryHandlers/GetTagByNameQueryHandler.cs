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
    public class GetTagByNameQueryHandler : IRequestHandler<GetTagByNameQuery, Tag?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetTagByNameQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag?> Handle(GetTagByNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Tags
                 .AsNoTracking()
                 .Where(tag => tag.Name.Equals(request.TagName))
                 .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
