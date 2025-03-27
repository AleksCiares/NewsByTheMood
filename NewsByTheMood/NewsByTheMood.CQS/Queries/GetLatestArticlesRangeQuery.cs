using MediatR;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Queries
{
    public class GetLatestArticlesRangeQuery : IRequest<IEnumerable<Article>>   
    {
        public required short Positivity { get; set; }  
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
