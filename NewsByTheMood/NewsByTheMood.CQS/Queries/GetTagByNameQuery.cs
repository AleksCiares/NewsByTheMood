using MediatR;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTagByNameQuery : IRequest<Tag?>
    {
        public required string TagName { get; set; }
    }
}
