using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Model for editing or adding source
    public class SourceEditModel
    {
        [Required]
        public SourceModel Source { get; set; }

        public required TopicModel[] Topics { get; set; }

        public ArticlePreviewModel[] RelatedArticles { get; set; } = Array.Empty<ArticlePreviewModel>();
    }
}
