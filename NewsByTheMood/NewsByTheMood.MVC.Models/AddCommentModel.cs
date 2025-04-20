using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class AddCommentModel
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public required string Text { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string ArticleId { get; set; }
    }
}
