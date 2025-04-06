using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TopicService : ITopicService
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        private readonly IMediator _mediator;

        public TopicService(NewsByTheMoodDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<Topic?> GetByIdAsync(Int64 id, CancellationToken cancellationToken =default)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _mediator.Send(new GetTopicByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<Topic?> GetByNameAsync(string topicName, CancellationToken cancellationToken = default)
        {
            if (topicName.IsNullOrEmpty())
            {
                return null;
            }

            return await _mediator.Send(new GetTopicByNameQuery() { TopicName = topicName }, cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return Array.Empty<Topic>();
            }

            return await _mediator.Send(new GetTopicsRangeQuery()
            {
                Page = pageNumber,
                PageSize = pageSize
            }, cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetAllTopicsQuery(), cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetTopicsCountQuery(), cancellationToken);
        }

        public async Task<bool> IsExistsByNameAsync(string topicName, CancellationToken cancellationToken = default)
        {
            if (topicName.IsNullOrEmpty())
            {
                return false;
            }

            return await _mediator.Send(new IsExistsTopicByNameQuery() { TopicName = topicName }, cancellationToken);
        }

        public async Task<bool> AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            if(await IsExistsByNameAsync(topic.Name))
            {
                return false;
            }

            await _mediator.Send(new AddTopicCommand() { Topic = topic }, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            var existingTopic = await GetByIdAsync(topic.Id, cancellationToken);
            if (existingTopic == null)
            {
                return false;
            }
            if (await IsExistsByNameAsync(topic.Name, cancellationToken) && !existingTopic.Name.Equals(topic.Name))
            {
                return false;
            }

            await _mediator.Send(new UpdateTopicCommand() { Topic = topic }, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var existingTopic = await GetByIdAsync(id, cancellationToken);
            if (existingTopic == null)
            {
                return false;
            }
            if(existingTopic.Sources.Count > 0)
            {
                return false;
            }

            await _mediator.Send(new DeleteTopicCommand() { Topic = existingTopic }, cancellationToken);
            return true;
        }
    }
}
