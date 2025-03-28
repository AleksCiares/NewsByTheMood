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
    public class IsExistsTagByNameQueryHandler : IRequestHandler<IsExistsTagByNameQuery, bool>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public IsExistsTagByNameQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(IsExistsTagByNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Tags
                .AsNoTracking()
                .Where(tag => tag.Name.Equals(request.TagName))
                .AnyAsync(cancellationToken);
        }
    }
}
