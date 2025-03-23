using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticlePreviewModel
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string Title { get; set; }
     
        [Required]
        public required string SourceName { get; set; }

        [Required]
        public required string TopicName { get; set; }
    }
}
