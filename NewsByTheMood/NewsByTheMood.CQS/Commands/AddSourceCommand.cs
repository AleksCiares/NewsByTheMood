using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddSourceCommand : IRequest
    {
        public required Source Source { get; set; }
    }
}
