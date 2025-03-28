using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class DeleteTopicCommand : IRequest
    {
        public required Topic Topic { get; set; }
    }
}
