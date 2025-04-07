using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetSourcesCountQueryHandler : IRequestHandler<GetSourcesCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetSourcesCountQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetSourcesCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }
    }
}
