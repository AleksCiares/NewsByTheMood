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
    public class GetLatestArticlesRangeQueryHandler : IRequestHandler<GetLatestArticlesRangeQuery, IEnumerable<Article>>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetLatestArticlesRangeQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Article>> Handle(GetLatestArticlesRangeQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= request.Positivity)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);
        }
    }
}
