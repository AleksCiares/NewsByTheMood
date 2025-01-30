namespace NewsByTheMood.Services.DataProvider.DTO
{
    public class ArticleDTO
    {
        public Int64 Id { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string? Body { get; set; }
        public DateTime? PublishDate { get; set; }
        public short Positivity { get; set; }
        public int Rating { get; set; }
        public string? SourceName { get; set; }
        public string[]? ArticleTags { get; set; }
    }
}
