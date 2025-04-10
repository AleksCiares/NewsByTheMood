namespace NewsByTheMood.MVC.Models
{
    public class HeaderModel
    {
        public UserPreviewModel? UserPreview { get; set; } 
        public required TopicModel[] Topics { get; set; }
    }
}
