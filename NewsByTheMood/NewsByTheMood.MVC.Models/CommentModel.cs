namespace NewsByTheMood.MVC.Models
{
    public class CommentModel
    {
        public required string Text { get; set; }
        public required DateTime PublishDate { get; set; }
        public required string UserAvatar { get; set; }
        public required string UserDisplayName { get; set; }
        public required string UserName { get; set; }
        public required bool Deleted { get; set; }
    }
}
