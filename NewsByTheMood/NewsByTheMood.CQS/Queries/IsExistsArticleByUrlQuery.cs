using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class IsExistsArticleByUrlQuery : IRequest<bool>
    {
        public required string ArticleUrl { get; set; }
    }
}
