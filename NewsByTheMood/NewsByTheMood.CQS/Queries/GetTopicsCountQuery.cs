using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTopicsCountQuery : IRequest<int>
    {
    }
}
