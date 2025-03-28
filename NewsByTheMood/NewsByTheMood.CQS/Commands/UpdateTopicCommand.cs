using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class UpdateTopicCommand : IRequest
    {
        public required Topic Topic { get; set; }
    }
}
