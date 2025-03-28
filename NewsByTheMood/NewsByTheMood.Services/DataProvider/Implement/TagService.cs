using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using System.Collections.Generic;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TagService : ITagService
    {
        //private readonly NewsByTheMoodDbContext _dbContext;
        private readonly IMediator _mediatR;

        public TagService(/*NewsByTheMoodDbContext dbContext,*/ IMediator mediator)
        {
            //_dbContext = dbContext;
            _mediatR = mediator;
        }

        public async Task<Tag?> GetByNameAsync(string tagName, CancellationToken cancellationToken = default)
        {
            if (tagName.IsNullOrEmpty())
            {
                return null;
            }

            return await _mediatR.Send(new GetTagByNameQuery() { TagName = tagName }, cancellationToken);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _mediatR.Send(new GetAllTagsQuery(), cancellationToken);
        }

        public async Task<bool> IsExistsByNameAsync(string tagName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return false;
            }

            return await _mediatR.Send(new IsExistsTagByNameQuery() { TagName = tagName }, cancellationToken);
        }

        public async Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
        {
           await _mediatR.Send(new AddTagCommand() { Tag = tag }, cancellationToken);
        }
    }
}
