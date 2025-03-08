namespace NewsByTheMood.Data.Entities
{
    public class User
    {
        // [Key]
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayedName { get; set; }
        public string PasswordHash { get; set; }
        public short PreferedPositivity { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsVerified { get; set; }
        public string AvatarUrl { get; set; }
        // FK
        public Int64 RightId { get; set; }
        // nav property
        public Right Right { get; set; }

        // nav property
        public List<Comment> Comments { get; set; }
        // nav property
        public List<Topic> Topics { get; set; }

    }
}
