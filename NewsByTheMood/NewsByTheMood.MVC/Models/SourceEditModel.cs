using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class SourceEditModel
    {
        [Required]
        public required SourceModel Source { get; set; }

        [Required]
        public required TopicModel[] Topics { get; set; }

        [Required]
        public required ArticlePreviewModel[] RelatedArticles { get; set; } = Array.Empty<ArticlePreviewModel>();
    }
}
