using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TopicService : ITopicService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TopicService> _logger; 

        public TopicService(IMediator mediator, ILogger<TopicService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Topic?> GetByIdAsync(Int64 id, CancellationToken cancellationToken =default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Topic id is less than or equal to 0. Id: {id}");
                return null;
            }

            return await _mediator.Send(new GetTopicByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<Topic?> GetByNameAsync(string topicName, CancellationToken cancellationToken = default)
        {
            if (topicName.IsNullOrEmpty())
            {
                _logger.LogWarning($"Topic name is null or empty. TopicName: {topicName}");
                return null;
            }

            return await _mediator.Send(new GetTopicByNameQuery() { TopicName = topicName }, cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                _logger.LogWarning($"Page size or page number is less than or equal to 0. PageSize: {pageSize}, PageNumber: {pageNumber}");
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
                _logger.LogWarning($"Topic name is null or empty. TopicName: {topicName}");
                return false;
            }

            return await _mediator.Send(new IsExistsTopicByNameQuery() { TopicName = topicName }, cancellationToken);
        }

        public async Task<bool> AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            if(await IsExistsByNameAsync(topic.Name))
            {
                _logger.LogWarning($"Topic with name {topic.Name} already exists.");
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
                _logger.LogWarning($"Topic with id {topic.Id} does not exist.");
                return false;
            }
            if (await IsExistsByNameAsync(topic.Name, cancellationToken) && !existingTopic.Name.Equals(topic.Name))
            {
                _logger.LogWarning($"Topic with name {topic.Name} already exists.");
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
                _logger.LogWarning($"Topic with id {id} does not exist.");
                return false;
            }
            if(existingTopic.Sources.Count > 0)
            {
                _logger.LogWarning($"Topic with id {id} has sources. Cannot delete.");
                return false;
            }

            await _mediator.Send(new DeleteTopicCommand() { Topic = existingTopic }, cancellationToken);
            return true;
        }
    }
}
