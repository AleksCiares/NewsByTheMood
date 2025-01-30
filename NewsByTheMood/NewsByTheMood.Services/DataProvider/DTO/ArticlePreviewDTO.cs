using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.Services.DataProvider.DTO
{
    public class ArticlePreviewDTO
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public short Positivity { get; set; }
        public int Rating { get; set; }
        public string? SourceName { get; set; }
    }
}
