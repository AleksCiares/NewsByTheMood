using MediatR;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesRangeQuery : IRequest<IEnumerable<Article>>   
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
        public required short Positivity { get; set; }  
        public required bool IgnoreActivity { get; set; }
        public IEnumerable<long>? TopicIds { get; set; }
    }
}
