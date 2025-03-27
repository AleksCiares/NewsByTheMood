using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesRangeByTopicQuery : IRequest<IEnumerable<Article>>
    {
        public short Positivity { get; set; } = 0;
        public long TopicId { get; set; }   
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
