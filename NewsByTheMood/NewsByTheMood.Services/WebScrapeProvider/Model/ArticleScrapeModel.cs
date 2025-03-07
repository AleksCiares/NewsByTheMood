using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.Services.WebScrapeProvider.Model
{
    internal class ArticleScrapeModel
    {
        public string Title { get; set; }
        public string? PreviewImgUrl { get; internal set; }
        public string Body { get; internal set; }
        public string? PublishDate { get; internal set; }
        public string[]? Tags { get; internal set; }
    }
}
