using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetCommentByIdQuery : IRequest<Comment?>
    {
        public required long CommentId { get; set; }
    }
}
