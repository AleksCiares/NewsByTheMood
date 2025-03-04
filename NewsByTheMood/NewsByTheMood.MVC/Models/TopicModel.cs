using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Topic display model
    public class TopicModel
    {
        public string? Id { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Icon Css Class is too small or long (maximum is 100 characters)")]
        public string? IconCssClass { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is too small or long (maximum is 100 characters)")]
        public required string Name { get; set; }
    }
}
