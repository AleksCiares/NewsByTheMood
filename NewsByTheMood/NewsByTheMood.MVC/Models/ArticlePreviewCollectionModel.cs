using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticlePreviewCollectionModel
    {
        [Required]
        public required ArticlePreviewModel[] ArticleShortPreviews { get; set; }

        [Required]
        public required PageInfoModel PageInfo { get; set; }
    }
}
