﻿
namespace NewsByTheMood.MVC.Models
{
    // Model of the full information of the article (without comments, they are uploaded separately)
    public class ArticleDisplayModel
    {
        public required string Url { get; set; }
        public required string Title { get; set; }
        public string? PreviewImgUrl { get; set; }
        public string? Body { get; set; }
        public string? PublishDate { get; set; }
        public required short Positivity { get; set; }
        public required int Rating { get; set; }
        public required string SourceName { get; set; }
        public required string TopicName { get; set; }
        public required string[] ArticleTags { get; set; }
    }
}
