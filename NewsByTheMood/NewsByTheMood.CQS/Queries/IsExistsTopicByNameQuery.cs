using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class IsExistsTopicByNameQuery : IRequest<bool>
    {
        public required string TopicName { get; set; }
    }
}
