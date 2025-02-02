namespace NewsByTheMood.MVC.Models
{
    public class ArticleModel
    {
        public string Uri { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? PublishDate { get; set; }
        public short Positivity { get; set; }
        public int Rating { get; set; }
        public string? SourceName { get; set; }
        public string? TopicName { get; set; }
        public string[]? ArticleTags { get; set; }
    }
}
