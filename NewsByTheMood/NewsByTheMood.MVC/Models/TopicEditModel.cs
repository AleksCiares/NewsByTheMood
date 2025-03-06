using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class TopicEditModel
    {
        [Required]
        public required TopicModel Topic { get; set; }

        public SourcePreviewModel[]? RelatedSources { get; set; }
    }
}
