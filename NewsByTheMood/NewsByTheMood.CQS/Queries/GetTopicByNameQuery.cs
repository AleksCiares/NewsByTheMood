using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTopicByNameQuery : IRequest<Topic?>
    {
        public required string TopicName { get; set; }
    }
}
