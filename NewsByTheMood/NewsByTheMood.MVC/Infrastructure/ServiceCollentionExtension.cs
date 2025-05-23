using Serilog;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using Microsoft.AspNetCore.Identity;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Implement;
using NewsByTheMood.Services.Mappers;
using NewsByTheMood.Services.FileProvider.Abstract;
using NewsByTheMood.Services.FileProvider.Implement;
using NewsByTheMood.Services.FileProvider.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using NewsByTheMood.Services.EmailProvider.Implement;
using NewsByTheMood.Services.EmailProvider.Options;
using NewsByTheMood.Services.Options;
using NewsByTheMood.Services.ScrapeProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Implement;
using NewsByTheMood.Services.ArticleProccessingService;
using Hangfire;

namespace NewsByTheMood.MVC.Infrastructure
{
    public static class ServiceCollentionExtension
    {
        public static IServiceCollection RegisterLogger(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            services.AddSerilog();

            return services;
        }

        public static IServiceCollection RegisterDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<NewsByTheMoodDbContext>(opt => 
                opt.UseSqlServer(configuration.GetConnectionString("Default"))
            );

            return services;
        }

        public static IServiceCollection RegisterIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Int64>>(options =>
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

            return services;
        }

        public static IServiceCollection ConfigureCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/identity/account/login";
                options.AccessDeniedPath = "/identity/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            return services;
        }

        public static IServiceCollection RegisterDataProvider(this IServiceCollection services)
        {
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(sc => 
                sc.RegisterServicesFromAssembly(typeof(CQS.Commands.AddArticleCommand).Assembly)
            );

            return services;
        }

        public static IServiceCollection RegisterMappers(this IServiceCollection services)
        {
            services.AddTransient<ArticlesMapper>();
            services.AddTransient<CommentsMapper>();
            services.AddTransient<SourcesMapper>();
            services.AddTransient<TopicsMapper>();
            services.AddTransient<UsersMapper>();

            return services;
        }

        public static IServiceCollection RegisterFileProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseUserIcons"))
            {
                services.Configure<UserIconsOptions>(
                    configuration.GetSection(UserIconsOptions.Position));
                services.AddSingleton<IiconService, LocalIconService>();
            }
            else
            {
                services.AddSingleton<IiconService, EmptyIconService>();
            }

            return services;
        }

        public static IServiceCollection RegisterEmailProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseEmailSender"))
            {
                services.Configure<EmailOptions>(
                    configuration.GetSection(EmailOptions.Position));
                services.AddTransient<IEmailSender, PrettyEmailSender>();
            }
            else
            {
                services.AddTransient<IEmailSender, EmptyEmailSender>();
            }

            return services;
        }

        public static IServiceCollection RegisterScrapeProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<WebScrapeOptions>(
                configuration.GetSection(WebScrapeOptions.Position));
            services.AddTransient<IArticleScrapeService, ArticleScrapeService>();

            return services;
        }

        public static IServiceCollection RegisterArticleProccesing(this IServiceCollection services)
        {
            services.AddTransient<ArticleProccessingService>();

            return services;
        }

        public static IServiceCollection RegisterHangfire(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("Hangfire")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return services;
        }
    }
}
