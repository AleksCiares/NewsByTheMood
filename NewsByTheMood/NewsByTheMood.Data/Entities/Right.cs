using Microsoft.EntityFrameworkCore;

namespace NewsByTheMood.Data.Entities
{
    [Index(nameof(AccessLevel), IsUnique = true)]
    public class Right
    {
        // [Key]
        public Int64 Id { get; set; }
        public string AccessLevel { get; set; }

        // nav property
        public List<User> Users { get; set; }
    }
}
