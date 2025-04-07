using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetAllTopicsQuery : IRequest<IEnumerable<Topic>>
    {
    }
}
