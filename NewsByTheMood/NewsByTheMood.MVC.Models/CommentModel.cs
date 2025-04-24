namespace NewsByTheMood.MVC.Models
{
    public class CommentModel
    {
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public string UserAvatar { get; set; }
        public string UserDisplayName { get; set; }
        public string UserName { get; set; }
    }
}
