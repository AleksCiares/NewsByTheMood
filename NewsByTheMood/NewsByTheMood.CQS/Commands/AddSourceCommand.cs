using MediatR;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Commands
{
    public class AddSourceCommand : IRequest
    {
        public required Source Source { get; set; }
    }
}
