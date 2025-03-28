using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.QueryHandlers
{
    public class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Topic?>
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public GetTopicByIdQueryHandler(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Topic?> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var topic = await _dbContext.Topics
               .Where(topic => topic.Id == request.Id)
               .SingleOrDefaultAsync();

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
                .Where(topic => topic.Id == id)
                .SingleOrDefaultAsync();*/