using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetSourcesRangeQuery : IRequest<IEnumerable<Source>>
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
