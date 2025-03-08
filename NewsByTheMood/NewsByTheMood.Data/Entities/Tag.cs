namespace NewsByTheMood.Data.Entities
{
    public class Tag
    {
        // [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }

        // nav property
        public List<Article> Articles { get; set; }
    }
}
