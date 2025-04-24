using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class IsExistsArticleByIdQuery : IRequest<bool>
    {
        public required long Id { get; set; }
    }
}
