using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Model for editing or adding topic
    public class TopicEditModel
    {
        [Required]
        public required TopicModel Topic { get; set; }

        public required SourcePreviewModel[] RelatedSources { get; set; }
    }
}
