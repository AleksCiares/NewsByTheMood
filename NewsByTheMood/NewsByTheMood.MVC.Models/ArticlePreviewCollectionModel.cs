namespace NewsByTheMood.MVC.Models
{
    // Article preview and pagination display model
    public class ArticlePreviewCollectionModel
    {
        public required ArticlePreviewModel[] Articles { get; set; }
        public required PageInfoModel PageInfo { get; set; }
        public required string PageTitle { get; set; }
    }
}
