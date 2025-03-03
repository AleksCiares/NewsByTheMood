namespace NewsByTheMood.Data.Entities
{
    public class UserTopic
    {
        // [Key]
        public Int64 Id { get; set; }
        // FK
        public Int64 UserId { get; set; }
        // FK
        public Int64 TopicId { get; set; }

        // nav property
        public User User { get; set; }
        // nav property
        public Topic Topic { get; set; }
    }
}
