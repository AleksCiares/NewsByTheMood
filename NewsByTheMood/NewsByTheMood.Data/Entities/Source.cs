namespace NewsByTheMood.Data.Entities
{
    public class Source
    {
        // [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SurveyPeriod { get; set; }
        public bool IsRandomPeriod { get; set; }
        public string ArticleListPath { get; set; }
        public string ArticleItemPath { get; set; }
        public string ArticleUriPath { get; set; }
        public string ArticleTitlePath { get; set; }
        public string ArticleBodyPath { get; set; }
        public string? ArticlePdatePath { get; set; }
        public string? ArticleTagPath { get; set; }

        // FK
        public Int64 TopicId { get; set; }
        // nav property
        public Topic Topic { get; set; }
        // nav property
        public List<Article> Articles { get; set; }
    }
}
