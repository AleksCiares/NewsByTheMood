using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Commands
{
    public class AddArticlesRangeCommand : IRequest
    {
        public required IEnumerable<Article> Articles { get; set; }
    }
}
