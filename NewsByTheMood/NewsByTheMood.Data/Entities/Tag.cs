using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NewsByTheMood.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }

        // nav property
        public List<Article> Articles { get; set; }
    }
}
