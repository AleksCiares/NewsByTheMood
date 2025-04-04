﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class SourceService : ISourceService
    {
        //private readonly NewsByTheMoodDbContext _dbContext;
        private readonly IMediator _mediator;

        public SourceService(/*NewsByTheMoodDbContext dbContext,*/ IMediator mediator)
        {
            //_dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<Source?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _mediator.Send(new GetSourceByIdQuery() { Id = id }, cancellationToken);
        }

        public async Task<IEnumerable<Source>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
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
                return false;
            }

            return await _mediator.Send(new IsExistsSourceByNameQuery() { SourceName = sourceName }, cancellationToken);
        }

        public async Task<bool> AddAsync(Source source, CancellationToken cancellationToken = default)
        {
            if(await IsExistsByNameAsync(source.Name))
            {
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
                return false;
            }
            if (await IsExistsByNameAsync(source.Name) && !source.Name.Equals(sourceEntity.Name))
            {
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
                return false;
            }

            if (source.Articles != null && source.Articles.Count > 0)
            {
                //"Can not delete source with related articles. First of all delete all related articles"
                return false;
            }

            await _mediator.Send(new DeleteSourceCommand() { Source = source }, cancellationToken);
            return true;
        }
    }
}
