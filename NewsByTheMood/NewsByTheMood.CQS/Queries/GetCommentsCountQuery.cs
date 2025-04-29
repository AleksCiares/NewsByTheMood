using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetCommentsCountQuery : IRequest<int>
    {
        public long ArticleId { get; set; }
    }
}
