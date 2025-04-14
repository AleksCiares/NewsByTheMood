using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Editor", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<long>()
                    {
                        Name = roleName
                    });
                }
            }

            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin@12345678";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    DisplayedName = "Admin",
                    RegDate = DateTime.Now,
                    PreferedPositivity = 1,
                    AvatarUrl = "/storage/usericons/default.webp"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
