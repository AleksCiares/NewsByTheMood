using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesCountByTopicQuery : IRequest<int>
    {
        public short Positivity { get; set; } = 0;
        public long TopicId { get; set; }
    }
}
