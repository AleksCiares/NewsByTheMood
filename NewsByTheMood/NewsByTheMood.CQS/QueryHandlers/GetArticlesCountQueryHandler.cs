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
    public class GetArticlesCountQueryHandler : IRequestHandler<GetArticlesCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetArticlesCountQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public async Task<int> Handle(GetArticlesCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                 .AsNoTracking()
                 .Where(article => article.Positivity >= request.Positivity)
                 .CountAsync(cancellationToken);
        }
    }
}
