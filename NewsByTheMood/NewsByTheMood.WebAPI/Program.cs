
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Implement;
using NewsByTheMood.Services.Mappers;
using Serilog;

namespace NewsByTheMood.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Logging service
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Services.AddSerilog();
            Log.Information("Starting host...");

            builder.Services.AddControllers(opt => 
            {
                opt.Filters.Add(new ProducesAttribute("application/json"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt => 
            {
                var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
            });

            // Db provider service
            builder.Services.AddDbContext<NewsByTheMoodDbContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            // Data provider services
            // Article service
            builder.Services.AddScoped<IArticleService, ArticleService>();

            //CQS services
            builder.Services.AddMediatR(sc => sc.RegisterServicesFromAssembly(typeof(CQS.Commands.AddArticleCommand).Assembly));

            //Mapper services
            builder.Services.AddTransient<ArticlesMapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
