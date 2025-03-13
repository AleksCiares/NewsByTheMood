using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class ArticlesLoadController : Controller
    {
        private readonly IArticleScrapeService _articleLoadService;
        private readonly ISourceService _sourceService;

        public ArticlesLoadController(IArticleScrapeService articleLoadService, ISourceService sourceService)
        {
            _articleLoadService = articleLoadService;
            _sourceService = sourceService;
        }

        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> LoadArticles(string id)
        {
            var source = await _sourceService.GetByIdAsync(long.Parse(id));
            if (source == null)
            {
                return RedirectToAction("Index", "Sources");
            }

            await _articleLoadService.LoadArticles(source);

            return RedirectToAction("Index", "Sources");
        }
    }
}
