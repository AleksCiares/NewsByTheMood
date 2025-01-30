using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Article controller
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly int _articlePageSize = 10;

        public ArticleController(IArticleService articleService)
        {
            this._articleService = articleService;
        }

        public async Task<IActionResult> Index(int pageNumber)
        {
            var articles = await this._articleService.GetRangePreviewAsync(this._articlePageSize, pageNumber = 1);
            return View(articles);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
