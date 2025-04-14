using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesCountByFavoriteQuery : IRequest<int>
    {
        public short Positivity { get; set; } = 0;
        public required IEnumerable<long> TopicIds { get; set; } = Enumerable.Empty<long>();
    }
}
