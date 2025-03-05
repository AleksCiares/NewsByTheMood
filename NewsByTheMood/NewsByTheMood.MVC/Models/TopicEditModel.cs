using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class TopicEditModel
    {
        [Required]
        public required TopicModel Topic { get; set; }

        [Required]
        public required SourcePreviewModel[] RelatedSources { get; set; } = Array.Empty<SourcePreviewModel>();
    }
}
