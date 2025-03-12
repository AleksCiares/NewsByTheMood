namespace NewsByTheMood.Data.Entities
{
    public class Comment
    { 
        // [Key]
        public Int64 Id { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }
        // FK
        public Int64 ArticleId { get; set; }
        // FK
        public Int64 UserId { get; set; }

        // nav property
        public Article Article { get; set; }
        // nav property
        public User User { get; set; }
    }
}
