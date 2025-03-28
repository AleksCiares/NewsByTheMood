using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddTagCommand : IRequest
    {
        public required Tag Tag{ get; set; }
    }
}
