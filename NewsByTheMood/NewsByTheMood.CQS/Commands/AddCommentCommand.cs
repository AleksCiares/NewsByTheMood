using MediatR;

namespace NewsByTheMood.CQS.Commands
{
    public class AddCommentCommand : IRequest<long>
    {
        public required long UserId { get; set; }
        public required long ArticleId { get; set; }
        public required string Text { get; set; } 
    }
}
