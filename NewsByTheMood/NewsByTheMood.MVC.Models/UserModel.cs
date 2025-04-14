namespace NewsByTheMood.MVC.Models
{
    public class UserModel
    {
        public string DisplayedName { get; set; }
        public short PreferedPositivity { get; set; } = 0;
        public string AvatarUrl { get; set; }
        public List<long> TopicsIds { get; set; } = new List<long>();
    }
}
