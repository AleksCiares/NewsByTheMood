namespace NewsByTheMood.MVC.Models
{
    public class CommentSettingsModel
    {
        public required string Id { get; set; }
        public required string Text { get; set; }
        public required DateTime PublishDate { get; set; }
        public required string UserAvatar { get; set; }
        public required string UserDisplayName { get; set; }
        public required string UserName { get; set; }
    }
}
