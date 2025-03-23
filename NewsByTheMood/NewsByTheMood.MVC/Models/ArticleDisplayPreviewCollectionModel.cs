using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Article preview and pagination display model
    public class ArticleDisplayPreviewCollectionModel
    {
        public required ArticleDisplayPreviewModel[] ArticlePreviews { get; set; }
        public required PageInfoModel PageInfo { get; set; }
        public required string PageTitle { get; set; }
    }
}
