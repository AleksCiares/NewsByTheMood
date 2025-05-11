using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class SourceService : ISourceService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SourceService> _logger;

        public SourceService(IMediator mediator, ILogger<SourceService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Source?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Source id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return null;
            }

            var result = await _mediator.Send(new GetSourceByIdQuery() 
            { 
                Id = id 
            },
            cancellationToken);

            if (result != null)
            {
                _logger.LogDebug($"Source was fetched successfully. SourceId: {id}");
            }
            else
            {
                _logger.LogWarning($"No source was found. SourceId: {id}");
            }

            return result;
        }

        public async Task<IEnumerable<Source>> GetRangeAsync(
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                _logger.LogWarning($"Page/PageSize is less than or equal to 0. " +
                    $"Page: {pageNumber}, PageSize: {pageSize}. Proccess will be aborted.");
                return Array.Empty<Source>();
            }

            var sources = await _mediator.Send(new GetSourcesRangeQuery() 
            { 
                Page = pageNumber, 
                PageSize = pageSize 
            }, 
            cancellationToken);

            _logger.LogDebug($"{sources.Count()} sources were fetch successfully. {{ " +
                $"Page: {pageNumber}; " +
                $"PageSize: {pageSize}; }}");

            return sources;
        }

        public async Task<IEnumerable<Source>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var sources =  await _mediator.Send(new GetSourcesRangeQuery()
            { 
                Page = 0,
                PageSize = 0,
                GetAll = true
            }, 
            cancellationToken);

            _logger.LogDebug($"{sources.Count()} sources were fetch successfully. {{ " +
                $"Page: {0}; " +
                $"PageSize: {0}; +" +
                $"GetAll: true; }}");

            return sources;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var count = await _mediator.Send(new GetSourcesCountQuery(), cancellationToken);

            _logger.LogDebug($"{count} articles were found.");

            return count;
        }

        public async Task<bool> IsExistsByNameAsync(string sourceName, CancellationToken cancellationToken = default)
        {
            if (sourceName.IsNullOrEmpty())
            {
                _logger.LogWarning($"Source name is null or empty. SourceName: {sourceName}. Proccess aborted.");
                return false;
            }

           var isExists =  await _mediator.Send(new IsExistsSourceByNameQuery() 
           { 
               SourceName = sourceName 
           }, 
           cancellationToken);

            _logger.LogDebug($"Source with name {sourceName} exists: {isExists}");

            return isExists;
        }

        public async Task<bool> AddAsync(Source source, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding source... SourceName: {source.Name}.");

            if (await IsExistsByNameAsync(source.Name))
            {
                _logger.LogError($"Source with name {source.Name} already exists. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new AddSourceCommand() 
            { 
                Source = source 
            }, 
            cancellationToken);

            _logger.LogInformation($"Source was added successfully. SourceName: {source.Name}.");

            return true;
        }

        public async Task<bool> UpdateAsync(Source source, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating source... SourceId: {source.Id}.");

            var sourceEntity = await GetByIdAsync(source.Id, cancellationToken);

            if (sourceEntity == null)
            {
                _logger.LogError($"Source not found. SourceId: {source.Id}. Proccess aborted.");
                return false;
            }

            if (await IsExistsByNameAsync(source.Name) && !source.Name.Equals(sourceEntity.Name))
            {
                _logger.LogError($"Source with same name already exists. SourceName: {source.Name}. Proccess aborted.");
                return false;
            }

            await _mediator.Send(new UpdateSourceCommand() 
            { 
                Source = source 
            },
            cancellationToken);

            _logger.LogInformation($"Source was updated successfully. SourceId: {source.Id}");

            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Deleting source... SourceId: {id}.");

            if (id <= 0)
            {
                _logger.LogError($"Source id is less than or equal to 0. Id: {id}. Proccess aborted.");
                return false;
            }

            var source = await GetByIdAsync(id, cancellationToken);

            if (source == null) 
            {
                _logger.LogError($"Source not found. SourceId: {id}. Procces aborted.");
                return false;
            }

            if (source.Articles != null && source.Articles.Count > 0)
            {
                _logger.LogError($"Can not delete source with related articles. " +
                    $"First of all delete all related articles. SourceId: {id}; ArticleCount {source.Articles.Count}. " +
                    $"Procces aborted.");
                return false;
            }

            await _mediator.Send(new DeleteSourceCommand() 
            { 
                Source = source 
            }, 
            cancellationToken);

            _logger.LogInformation($"Source was deleted successfully. SourceId: {id}");

            return true;
        }
    }
}
