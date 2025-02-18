using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataObfuscator.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Home controller
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            this._articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var page = 1;
            var size = 10;
            short positivity = 7;
            var rating = 0;

            var articles = (await this._articleService.GetRangePreviewAsync(
                page,
                size,
                positivity,
                rating))? // replaced with mapper
                .Select(article => new ArticlePreviewModel()
                {
                    Id = article.Id.ToString(),
                    Title = article.Title,
                    PublishDate = article.PublishDate.ToString(),
                    Positivity = article.Positivity,
                    Rating = article.Rating,
                    SourceName = article.Source.Name,
                    TopicName = article.Source.Topic.Name
                })
                .ToArray();

            return View(articles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
