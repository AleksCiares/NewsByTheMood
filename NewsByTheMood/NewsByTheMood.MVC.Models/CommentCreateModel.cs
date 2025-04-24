using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class CommentCreateModel
    {
        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Comment cannot be longer than 500 characters and empty")]
        public string Text { get; set; }
    }
}
