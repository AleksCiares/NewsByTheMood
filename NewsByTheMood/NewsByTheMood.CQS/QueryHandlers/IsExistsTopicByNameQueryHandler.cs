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
    public class IsExistsTopicByNameQueryHandler : IRequestHandler<IsExistsTopicByNameQuery, bool>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public IsExistsTopicByNameQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(IsExistsTopicByNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Name.Equals(request.TopicName))
                .AnyAsync(cancellationToken);
        }
    }
}
