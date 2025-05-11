using MediatR;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TagService : ITagService
    {
        private readonly IMediator _mediatR;

        public TagService(IMediator mediator)
        {
            _mediatR = mediator;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _mediatR.Send(new GetAllTagsQuery(), cancellationToken);
        }
    }
}
