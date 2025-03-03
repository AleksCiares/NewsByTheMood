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
        public bool AcceptInsecureCerts { get; set; }
        public string PageElementLoaded { get; set; }
        public int PageLoadTimeout { get; set; }
        public int ElementLoadTimeout { get; set; }

        public string ArticleCollectionsPath { get; set; }
        public string ArticleItemPath { get; set; }
        public string ArticleUrlPath { get; set; }

        public string ArticleTitlePath { get; set; }
        public string? ArticlePreviewImgPath { get; set; }
        public string ArticleBodyCollectionsPath { get; set; }
        public string ArticleBodyItemPath { get; set; }
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
