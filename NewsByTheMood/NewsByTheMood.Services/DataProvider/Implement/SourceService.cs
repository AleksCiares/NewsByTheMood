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

        public async Task<Source?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Source id is less than or equal to 0. Id: {id}");
                return null;
            }

            return await _mediator.Send(new GetSourceByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<IEnumerable<Source>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                _logger.LogWarning($"Page size or page number is less than or equal to 0. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Array.Empty<Source>();
            }

            return await _mediator.Send(new GetSourcesRangeQuery() 
            { 
                Page = pageNumber, 
                PageSize = pageSize 
            }, cancellationToken);
        }

        public async Task<IEnumerable<Source>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetAllSourcesQuery(), cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetSourcesCountQuery(), cancellationToken);
        }

        public async Task<bool> IsExistsByNameAsync(string sourceName, CancellationToken cancellationToken = default)
        {
            if (sourceName.IsNullOrEmpty())
            {
                _logger.LogWarning($"Source name is null or empty. SourceName: {sourceName}");
                return false;
            }

            return await _mediator.Send(new IsExistsSourceByNameQuery() { SourceName = sourceName }, cancellationToken);
        }

        public async Task<bool> AddAsync(Source source, CancellationToken cancellationToken = default)
        {
            if(await IsExistsByNameAsync(source.Name))
            {
                _logger.LogWarning($"Source with name {source.Name} already exists.");
                return false;
            }

            await _mediator.Send(new AddSourceCommand() { Source = source }, cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(Source source, CancellationToken cancellationToken = default)
        {
            var sourceEntity = await GetByIdAsync(source.Id, cancellationToken);
            if (sourceEntity == null)
            {
                _logger.LogWarning($"Source with id {source.Id} not found.");
                return false;
            }
            if (await IsExistsByNameAsync(source.Name) && !source.Name.Equals(sourceEntity.Name))
            {
                _logger.LogWarning($"Source with name {source.Name} already exists.");
                return false;
            }

            await _mediator.Send(new UpdateSourceCommand() { Source = source }, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var source = await GetByIdAsync(id, cancellationToken);
            if (source == null) 
            {
                _logger.LogWarning($"Source with id {id} not found.");
                return false;
            }

            /*if (source.Articles != null && source.Articles.Count > 0)
            {
                //"Can not delete source with related articles. First of all delete all related articles"
                return false;
            }*/

            await _mediator.Send(new DeleteSourceCommand() { Source = source }, cancellationToken);
            return true;
        }
    }
}
