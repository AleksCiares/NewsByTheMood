using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class DeleteArticleCommand : IRequest
    {
        public required Article Article { get; set; }
    }
}
