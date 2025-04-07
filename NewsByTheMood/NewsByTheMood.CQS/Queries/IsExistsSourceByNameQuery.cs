using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class IsExistsSourceByNameQuery : IRequest<bool>
    {
        public required string SourceName { get; set; }
    }
}
