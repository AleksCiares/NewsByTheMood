using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Article preview display model
    public class ArticlePreviewModel
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public string? PreviewImgUrl { get; set; }
        public string? PublishDate { get; set; }
        public required short Positivity { get; set; }
        public required int Rating { get; set; }
        public required string SourceName { get; set; }
        public required string SourceUrl { get; set; }
        public required string TopicName { get; set; }
    }
}
