using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.DTO
{
    public class CommentDTO
    {
        public Int64 Id { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }
        public string UserName { get; set; }
        public string UserDisplayedName { get; set; }
        public string AvatarUrl { get; set; }
    }
}
