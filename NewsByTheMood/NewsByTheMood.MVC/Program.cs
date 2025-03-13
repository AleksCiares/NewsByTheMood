using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.MVC.Options;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Implement;
using NewsByTheMood.Services.FileProvider.Abstract;
using NewsByTheMood.Services.FileProvider.Implement;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.ScrapeProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Implement;

namespace NewsByTheMood.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Db provider service
            builder.Services.AddDbContext<NewsByTheMoodDbContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default1")));

            // Configuration
            /*builder.Services.Configure<SpoofOptions>(
                builder.Configuration.GetSection(SpoofOptions.Position));*/


            // Data provider services
            // Article service
            builder.Services.AddScoped<IArticleService, ArticleService>();
            // Comment service
            builder.Services.AddScoped<ICommentService, CommentService>();
            // Source service
            builder.Services.AddScoped<ISourceService, SourceService>();
            // Tag service
            builder.Services.AddScoped<ITagService, TagService>();
            // Topic service
            builder.Services.AddScoped<ITopicService, TopicService>();
            // User service
            builder.Services.AddScoped<IUserService, UserService>();

            // File provider services
            // Icons service
            if (builder.Configuration.GetValue<bool>("UseUserIcons"))
            {
                builder.Services.Configure<UserIconsOptions>(
                    builder.Configuration.GetSection(UserIconsOptions.Position));
                builder.Services.AddSingleton<IiconService, LocalIconService>();
            }
            else
            {
                builder.Services.AddSingleton<IiconService, EmptyIconService>();
            }

            // Scrape provider services
            builder.Services.Configure<WebScrapeOptions>(
                builder.Configuration.GetSection(WebScrapeOptions.Position));
            // Article scrape service
            builder.Services.AddScoped<IArticleScrapeService, ArticleScrapeService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapAreaControllerRoute(
                name: "AreaSettings",
                areaName: "Settings",
                pattern: "settings/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
