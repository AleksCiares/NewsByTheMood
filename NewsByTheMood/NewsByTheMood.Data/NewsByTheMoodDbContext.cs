using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Data
{
    public class NewsByTheMoodDbContext : IdentityDbContext<User, IdentityRole<Int64>, Int64>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public NewsByTheMoodDbContext(DbContextOptions<NewsByTheMoodDbContext> options)
            :base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Right>().HasData(
                new Right { Id = 1, AccessLevel = "Administrator"},
                new Right { Id = 2, AccessLevel = "Moderator"},
                new Right { Id = 3, AccessLevel = "Content editor"},
                new Right { Id = 4, AccessLevel = "User"});

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "Administrator",
                Email = "test@test.com",
                DisplayedName = "NewsByTheMood Admin",
                PasswordHash = "Qwerty",
                PreferedPositivity = 0,
                RegDate = new DateTime(2025, 3, 12, 12, 35, 21, 312, DateTimeKind.Local),
                IsVerified = true,
                AvatarUrl = "~/images/newsbythemood-logo.webp",
                RightId = 1
            }); 
        }*/
    }
}
