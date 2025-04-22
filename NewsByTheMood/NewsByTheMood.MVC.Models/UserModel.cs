namespace NewsByTheMood.MVC.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public short PreferedPositivity { get; set; } = 0;
        public List<long> TopicsIds { get; set; } = new List<long>();
    }
}
