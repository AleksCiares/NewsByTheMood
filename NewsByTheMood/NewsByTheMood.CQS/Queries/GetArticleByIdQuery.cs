using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticleByIdQuery : IRequest<Article>
    {
        public required long Id { get; set; }
    }
}
