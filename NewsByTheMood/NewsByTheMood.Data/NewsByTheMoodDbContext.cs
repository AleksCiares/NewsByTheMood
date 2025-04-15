using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsByTheMood.Core.Settings;
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

        public async Task SeedData(IServiceProvider serviceProvider, InitialData initialData)
        {
            await SeedRoles(serviceProvider);
            await SeedUsers(serviceProvider, initialData.User);
        }

        private async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
            foreach (var roleName in AccessLevels.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<long>()
                    {
                        Name = roleName
                    });
                }
            }
        }

        private async Task SeedUsers(IServiceProvider serviceProvider, InitialUser user)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var admin = await userManager.FindByNameAsync(user.UserName);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = true,
                    DisplayedName = user.DisplayedName,
                    RegDate = DateTime.Now,
                    PreferedPositivity = 0,
                    AvatarUrl = "/storage/usericons/default/default.webp",
                };

                var result = await userManager.CreateAsync(admin, user.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AccessLevels.Admininistrator);
                }
            }
        }
    }
}
