using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class DeleteCommentsRangeCommand : IRequest<long[]>
    {
        public required long[] Ids { get; set; }
    }
}
