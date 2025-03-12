namespace NewsByTheMood.Services.WebScrapeProvider.Models
{
    /// <summary>
    /// Model for scraped article
    /// </summary>
    public class ArticleScrapeModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string? PreviewImgUrl { get; set; }
        public string Body { get; set; }
        public DateTime? PublishDate { get;  set; }
        public string[]? Tags { get; set; }
    }
}
