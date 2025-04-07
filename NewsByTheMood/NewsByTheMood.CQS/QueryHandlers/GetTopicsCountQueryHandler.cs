using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetTopicsCountQueryHandler : IRequestHandler<GetTopicsCountQuery, int>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetTopicsCountQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetTopicsCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }
    }
}
