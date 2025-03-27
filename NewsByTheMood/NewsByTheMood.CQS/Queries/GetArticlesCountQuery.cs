using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Queries
{
    public class GetArticlesCountQuery : IRequest<int>
    {
        public short Positivity { get; set; } = 0;
    }
}
