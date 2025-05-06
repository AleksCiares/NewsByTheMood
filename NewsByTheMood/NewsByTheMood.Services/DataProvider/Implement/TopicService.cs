using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using OpenQA.Selenium.BiDi.Modules.Script;

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

        public async Task<Topic?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Topic id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return null;
            }

            var result = await _mediator.Send(new GetTopicByIdQuery() 
            { 
                Id = id 
            }, 
            cancellationToken);

            if (result != null)
            {
                _logger.LogDebug($"Topic was fetched successfully. TopicId: {id}");
            }
            else
            {
                _logger.LogWarning($"No topic was found. TopicId: {id}");
            }

            return result;
        }

        public async Task<Topic?> GetByNameAsync(string topicName, CancellationToken cancellationToken = default)
        {
            if (topicName.IsNullOrEmpty())
            {
                _logger.LogWarning($"Topic name is null or empty. TopicName: {topicName}. Proccess aborted.");
                return null;
            }

            var result = await _mediator.Send(new GetTopicByNameQuery() 
            { 
                TopicName = topicName 
            }, 
            cancellationToken);

            if (result != null)
            {
                _logger.LogDebug($"Topic was fetched successfully. TopicName: {topicName}");
            }
            else
            {
                _logger.LogWarning($"No topic was found. TopicName: {topicName}");
            }

            return result;
        }

        public async Task<IEnumerable<Topic>> GetRangeAsync(
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                _logger.LogWarning($"Page/PageSize is less than or equal to 0. " +
                     $"Page: {pageNumber}, PageSize: {pageSize}. Proccess will be aborted.");
                return Array.Empty<Topic>();
            }

            var topics = await _mediator.Send(new GetTopicsRangeQuery()
            {
                Page = pageNumber,
                PageSize = pageSize
            }, 
            cancellationToken);

            _logger.LogDebug($"{topics.Count()} sources were fetch successfully. {{ " +
                $"Page: {pageNumber}; " +
                $"PageSize: {pageSize}; }}");

            return topics;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var topics = await _mediator.Send(new GetTopicsRangeQuery() 
            { 
                Page = 0,
                PageSize = 0,
                GetAll = true
            }, 
            cancellationToken);

            _logger.LogDebug($"{topics.Count()} sources were fetch successfully. {{ " +
                $"Page: {0}; " +
                $"PageSize: {0}; +" +
                $"GetAll: true; }}");

            return topics;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var count = await _mediator.Send(new GetTopicsCountQuery(), cancellationToken);

            _logger.LogDebug($"{count} topics were found.");

            return count;
        }

        public async Task<bool> IsExistsByNameAsync(string topicName, CancellationToken cancellationToken = default)
        {
            if (topicName.IsNullOrEmpty())
            {
                _logger.LogWarning($"Topic name is null or empty. TopicName: {topicName}. Proccess aborted.");
                return false;
            }

            var isExists = await _mediator.Send(new IsExistsTopicByNameQuery() 
            { 
                TopicName = topicName
            }, 
            cancellationToken);

            _logger.LogDebug($"Topic with name {topicName} exists: {isExists}");

            return isExists;
        }

        public async Task<bool> AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding topic... TopicName: {topic.Name}.");

            if (await IsExistsByNameAsync(topic.Name))
            {
                _logger.LogError($"Topic with name {topic.Name} already exists. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new AddTopicCommand() 
            { 
                Topic = topic 
            }, 
            cancellationToken);

            _logger.LogInformation($"Topic was added successfully. TopicName: {topic.Name}.");

            return true;
        }

        public async Task<bool> UpdateAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating topic... TopicId: {topic.Id}.");

            var existingTopic = await GetByIdAsync(topic.Id, cancellationToken);

            if (existingTopic == null)
            {
                _logger.LogError($"Topic not found. TopicId: {topic.Id}. Proccess aborted.");
                return false;
            }

            if (await IsExistsByNameAsync(topic.Name, cancellationToken) && !existingTopic.Name.Equals(topic.Name))
            {
                _logger.LogError($"Topic with same name already exists. TopicName: {topic.Name}. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new UpdateTopicCommand() 
            { 
                Topic = topic 
            }, 
            cancellationToken);

            _logger.LogInformation($"Topic was updated successfully. TopicId: {topic.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting topic... TopicId: {id}.");

            if (id <= 0)
            {
                _logger.LogError($"Topic id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return false;
            }

            var existingTopic = await GetByIdAsync(id, cancellationToken);

            if (existingTopic == null)
            {
                _logger.LogError($"Topic not found. TopicId: {id}. Procces aborted.");
                return false;
            }

            if(existingTopic.Sources.Count > 0)
            {
                _logger.LogError($"Can not delete topic with related sources. " +
                    $"First of all delete all related sources. TopicId: {id}; SourceCount {existingTopic.Sources.Count}. " +
                    $"Procces aborted.");
                return false;
            }

            await _mediator.Send(new DeleteTopicCommand() 
            { 
                Topic = existingTopic 
            }, 
            cancellationToken);

            _logger.LogInformation($"Topic was deleted successfully. TopicId: {id}");

            return true;
        }
    }
}
