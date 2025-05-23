using NewsByTheMood.Data;
using Serilog;
using Hangfire;
using NewsByTheMood.MVC.Infrastructure;

namespace NewsByTheMood.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.RegisterLogger(builder.Configuration);
                Log.Information("Starting host...");

                builder.Services.RegisterDbContext(builder.Configuration);
                builder.Services.RegisterIdentity();
                builder.Services.ConfigureCookie();

                builder.Services.RegisterDataProvider();
                builder.Services.RegisterMediatR();
                builder.Services.RegisterMappers();

                builder.Services.RegisterScrapeProvider(builder.Configuration);
                builder.Services.RegisterArticleProccesing();

                builder.Services.RegisterFileProvider(builder.Configuration);
                builder.Services.RegisterEmailProvider(builder.Configuration);

                builder.Services.RegisterHangfire(builder.Configuration);

                builder.Services.AddControllersWithViews();
                builder.Services.AddRazorPages();
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

                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new HangfireAdminAuthorizationFilter() }
                });

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
