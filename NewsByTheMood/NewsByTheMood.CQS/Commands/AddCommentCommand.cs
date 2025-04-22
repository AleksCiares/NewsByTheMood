using MediatR;

namespace NewsByTheMood.CQS.Commands
{
    public class AddCommentCommand : IRequest
    {
        public required Int64 UserId { get; set; }
        public required Int64 ArticleId { get; set; }
        public required string Text { get; set; } 
    }
}
