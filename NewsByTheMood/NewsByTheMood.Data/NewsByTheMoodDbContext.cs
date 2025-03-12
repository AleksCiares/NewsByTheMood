using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Data
{
    public class NewsByTheMoodDbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }

        public NewsByTheMoodDbContext(DbContextOptions<NewsByTheMoodDbContext> options)
            :base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Right>().HasData(
                new Right { Id = 1, AccessLevel = "Administrator"},
                new Right { Id = 2, AccessLevel = "Moderator"},
                new Right { Id = 1, AccessLevel = "Content editor"},
                new Right { Id = 1, AccessLevel = "User"});
            base.OnModelCreating(modelBuilder);
        }
    }
}
