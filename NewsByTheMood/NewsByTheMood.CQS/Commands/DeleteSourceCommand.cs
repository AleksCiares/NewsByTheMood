using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class DeleteSourceCommand : IRequest
    {
        public required Source Source { get; set; }
    }
}
