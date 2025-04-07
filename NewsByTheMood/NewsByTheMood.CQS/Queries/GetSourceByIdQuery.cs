using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetSourceByIdQuery : IRequest<Source?>
    {
        public required Int64 Id { get; set; }
    }
}
