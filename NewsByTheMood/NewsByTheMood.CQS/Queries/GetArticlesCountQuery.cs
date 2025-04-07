using MediatR;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesCountQuery : IRequest<int>
    {
        public short Positivity { get; set; } = 0;
    }
}
