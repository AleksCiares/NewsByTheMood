using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetCommentsRangeByArticleIdQuery : IRequest<IEnumerable<Comment>>
    {
        public required long ArticleId { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }
}
