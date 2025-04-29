using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesCountQuery : IRequest<int>
    {
        public required short Positivity { get; set; }
        public required bool IgnoreActivity { get; set; }
        public IEnumerable<long>? TopicIds { get; set; }
    }
}
