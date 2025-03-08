namespace NewsByTheMood.Data.Entities
{
    public class Article
    {
        // [Key]
        public Int64 Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string? PreviewImgUrl { get; set; }
        public string? Body { get; set; }
        public DateTime? PublishDate { get; set; }
        public short Positivity { get; set; }
        public int Rating { get; set; }
        // FK
        public Int64 SourceId { get; set; }

        // nav property
        public Source Source { get; set; }
        // nav property
        public List<Tag> Tags { get; set; }
        // nav property
        public List <Comment> Comments { get; set; }

    }
}
