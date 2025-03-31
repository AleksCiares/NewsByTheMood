using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    public class ArticleSettingsCollectionModel
    {
        [Required]
        public required ArticleSettingsPreviewModel[] Articles { get; set; }

        [Required]
        public required PageInfoModel PageInfo { get; set; }
    }
}
