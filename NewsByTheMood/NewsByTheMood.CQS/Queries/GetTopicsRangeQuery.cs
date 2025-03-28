using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTopicsRangeQuery : IRequest<IEnumerable<Topic>>
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
