using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class TopicSettingsEditModel
    {
        [Required]
        public required TopicSettingsModel Topic { get; set; }

        [Required]
        public required int RelatedSourceCount { get; set; }
    }
}
