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
    public class GetTopicByNameQueryHandler : IRequestHandler<GetTopicByNameQuery, Topic?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetTopicByNameQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Topic?> Handle(GetTopicByNameQuery request, CancellationToken cancellationToken)
        {
            var topic = await _dbContext.Topics
                 .Where(topic => topic.Name.Equals(request.TopicName))
                 .SingleOrDefaultAsync(cancellationToken);

            if (topic != null)
            {
                await _dbContext.Entry(topic)
                    .Collection(topic => topic.Sources)
                    .LoadAsync(cancellationToken);

                _dbContext.Entry(topic)
                    .State = EntityState.Detached;

                return topic;
            }
            else
            {
                return null;
            }
        }
    }
}
/*return await _dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Name.Equals(topicName))
                .SingleOrDefaultAsync();*/
