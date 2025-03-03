namespace NewsByTheMood.Data.Entities
{
    public class ArticleTag
    {
        // [Key]
        public Int64 Id { get; set; }
        // FK
        public Int64 ArticleId { get; set; }
        // FK
        public Int64 TagId { get; set; }

        // nav property
        public Article Article { get; set; }
        // nav property
        public Tag Tag { get; set; }
    }
}
