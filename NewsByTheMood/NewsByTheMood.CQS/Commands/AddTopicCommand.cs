using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddTopicCommand : IRequest
    {
        public required Topic Topic { get; set; }
    }
}
