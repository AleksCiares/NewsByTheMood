using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.MVC.Options;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Implement;
using NewsByTheMood.Services.FileProvider.Abstract;
using NewsByTheMood.Services.FileProvider.Implement;
using NewsByTheMood.Services.MVC.Mappers;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.ScrapeProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Implement;
using NewsByTheMood.Services.EmailProvider;
using Serilog;
using Microsoft.AspNetCore.Identity;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.Mappers;

namespace NewsByTheMood.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Logging service
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();
                builder.Services.AddSerilog();
                Log.Information("Starting host...");

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddRazorPages();

                // Db provider service
                builder.Services.AddDbContext<NewsByTheMoodDbContext>(
                    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default1")));

                // Identity provider service
                builder.Services.AddIdentity<User, IdentityRole<Int64>>(options => 
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequiredLength = 12;
                        options.Password.RequireDigit = true;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                    })
                    .AddEntityFrameworkStores<NewsByTheMoodDbContext>()
                    .AddDefaultTokenProviders();

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/identity/account/login";
                    options.AccessDeniedPath = "/identity/account/accessdenied";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });

                // Auth service
                /*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie("NewsByTheMood", options =>
                    {
                        options.LoginPath = "/Identity/Account/Login";
                        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    });*/

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

                //CQS services
                builder.Services.AddMediatR(sc => sc.RegisterServicesFromAssembly(typeof(CQS.Commands.AddArticleCommand).Assembly));

                //Mapper services
                builder.Services.AddTransient<ArticlesMapper>();
                builder.Services.AddTransient<SourcesMapper>();
                builder.Services.AddTransient<TopicsMapper>();
                builder.Services.AddTransient<UsersMapper>();

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

                // Email provider services
                if (builder.Configuration.GetValue<bool>("UseEmailSender"))
                {
                    builder.Services.Configure<EmailOptions>(
                        builder.Configuration.GetSection(EmailOptions.Position));
                    builder.Services.AddTransient<IEmailSender, PrettyEmailSender>();
                }
                else
                {
                    builder.Services.AddTransient<IEmailSender, EmptyEmailSender>();
                }

                // Scrape provider services
                builder.Services.Configure<WebScrapeOptions>(
                    builder.Configuration.GetSection(WebScrapeOptions.Position));
                builder.Services.AddScoped<IArticleScrapeService, ArticleScrapeService>();

                // Spoof provider services
                /*builder.Services.Configure<SpoofOptions>(
                    builder.Configuration.GetSection(SpoofOptions.Position));*/

                // Routing service
                builder.Services.AddRouting(options => options.LowercaseUrls = true);
                
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

                app.UseAuthentication();
                app.UseAuthorization();

                // Map settings for application
                app.MapAreaControllerRoute(
                    name: "settings_area",
                    areaName: "Settings",
                    pattern: "settings/{controller=Home}/{action=Index}/{id?}");
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                app.MapRazorPages();

                // Seed database with init datas
                using (var scope = app.Services.CreateScope())
                {
                   await scope.ServiceProvider.GetRequiredService<NewsByTheMoodDbContext>()
                        .SeedData(scope.ServiceProvider, new Core.Settings.InitialData()
                        { 
                           User = new Core.Settings.InitialUser()
                           { 
                               DisplayedName = "Admin",
                               UserName = "SuperPuperAdmin",
                               Email = "admin@admin.com",
                               Password = "SuperPuperAdmin1234567890!",
                           }
                        });
                }

                Log.Information("Host Started");
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
