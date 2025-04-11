using Microsoft.AspNetCore.Identity;

namespace NewsByTheMood.Data.Entities
{
    public class User : IdentityUser<Int64>
    {
        public string DisplayedName { get; set; } = "Temp";
        public short PreferedPositivity { get; set; } = 1;
        public DateTime RegDate { get; set; }
        public string AvatarUrl { get; set; } = "Temp";

        // nav property
        public List<Comment> Comments { get; set; }
        // nav property
        public List<Topic> Topics { get; set; }
    }
}
