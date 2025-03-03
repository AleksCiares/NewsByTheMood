using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Topic display model
    public class TopicModel
    {
        public required string Id { get; set; }

        public string? IconCssClass { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too small or long (maximum is 100 characters)")]
        public required string Name { get; set; }
    }
}
