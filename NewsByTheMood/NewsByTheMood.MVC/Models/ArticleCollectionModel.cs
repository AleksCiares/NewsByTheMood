using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.MVC.Models
{
    // Article preview and pagination display model
    public class ArticleCollectionModel
    {
        public required ArticlePreviewModel[] ArticlePreviews { get; set; }
        public required PageInfoModel PageInfo { get; set; }
    }
}
