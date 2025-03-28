using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Queries
{
    public class IsExistsTagByNameQuery : IRequest<bool>
    {
        public required string TagName { get; set; }    
    }
}
