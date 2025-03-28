using MediatR;
using NewsByTheMood.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.CQS.Queries
{
    public class GetTopicByNameQuery : IRequest<Topic?>
    {
        public required string TopicName { get; set; }
    }
}
