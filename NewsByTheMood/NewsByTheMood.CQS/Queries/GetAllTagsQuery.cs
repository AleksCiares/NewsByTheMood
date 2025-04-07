using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetAllTagsQuery : IRequest<IEnumerable<Tag>>
    {
    }
}
