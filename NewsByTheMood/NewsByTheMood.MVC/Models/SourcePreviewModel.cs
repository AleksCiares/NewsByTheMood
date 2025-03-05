namespace NewsByTheMood.MVC.Models
{
    // Source preview display model
    public class SourcePreviewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string Topic { get; set; }
        public required int ArticleAmmount { get; set; }
    }
}
