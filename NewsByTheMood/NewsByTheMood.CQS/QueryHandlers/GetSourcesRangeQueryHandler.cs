using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

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
            IQueryable<Source> query = _dbContext.Sources
                .AsNoTracking()
                .Include(source => source.Topic)
                .Include(source => source.Articles)
                .OrderByDescending(source => source.Id);

            if (!request.GetAll)
            {
                query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            }

            return await query.ToArrayAsync(cancellationToken);
        }
    }
}
