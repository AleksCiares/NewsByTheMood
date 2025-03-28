using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTopicByIdQuery : IRequest<Topic>
    {
        public required long Id { get; set; }
    }
}
