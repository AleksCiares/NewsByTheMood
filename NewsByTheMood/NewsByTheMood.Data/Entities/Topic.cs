using Microsoft.EntityFrameworkCore;

namespace NewsByTheMood.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Topic
    {
        // [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string? IconCssClass { get; set; }

        // nav property
        public List<Source> Sources { get; set; }
        // nav property
        public List<User> Users { get; set; }
    }
}
