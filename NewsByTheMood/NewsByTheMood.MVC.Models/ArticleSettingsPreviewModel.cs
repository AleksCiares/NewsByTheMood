namespace NewsByTheMood.MVC.Models
{
    public class ArticleSettingsPreviewModel
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string SourceName { get; set; }
        public required string TopicName { get; set; }
        public required bool IsActive { get; set; }
        public required bool FailedLoaded { get; set; }
    }
}
