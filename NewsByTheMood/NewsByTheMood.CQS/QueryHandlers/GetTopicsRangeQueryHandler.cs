using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

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
            IQueryable<Topic> query = _dbContext.Topics
                .AsNoTracking()
                .Include(topic => topic.Sources)
                .OrderByDescending(topic => topic.Id);

            if (!request.GetAll)
            {
                query = query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            }

            return await query.ToArrayAsync(cancellationToken);
        }
    }
}
