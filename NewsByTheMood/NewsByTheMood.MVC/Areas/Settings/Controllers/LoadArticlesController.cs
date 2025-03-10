using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.DataLoadProvider.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class LoadArticlesController : Controller
    {
        private readonly IArticleLoadService _articleLoadService;
        private readonly ISourceService _sourceService;

        public LoadArticlesController(IArticleLoadService articleLoadService, ISourceService sourceService)
        {
            _articleLoadService = articleLoadService;
            _sourceService = sourceService;
        }

        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Load(string id)
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
