using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesRangeByFavoriteQuery : IRequest<IEnumerable<Article>>
    {
        public short Positivity { get; set; } = 0;
        public required IEnumerable<long> TopicIds { get; set; } = Enumerable.Empty<long>();
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
