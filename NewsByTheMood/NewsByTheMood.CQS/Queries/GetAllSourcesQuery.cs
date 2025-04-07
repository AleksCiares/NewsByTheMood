using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetAllSourcesQuery : IRequest<IEnumerable<Source>>
    {
    }
}
