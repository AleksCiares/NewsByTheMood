using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddArticleCommand : IRequest
    {
        public required Article Article { get; set; }
    }
}
