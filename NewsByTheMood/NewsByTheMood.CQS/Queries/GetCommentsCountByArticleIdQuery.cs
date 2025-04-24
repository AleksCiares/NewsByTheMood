using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetCommentsCountByArticleIdQuery : IRequest<int>
    {
        public long ArticleId { get; set; }
    }
}
