using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/[controller]/[action]")]
    public class ArticlesLoadController : Controller
    {
        private readonly IArticleScrapeService _articleLoadService;
        private readonly ISourceService _sourceService;
        private readonly ILogger<ArticlesLoadController> _logger;

        public ArticlesLoadController(IArticleScrapeService articleLoadService, ISourceService sourceService, ILogger<ArticlesLoadController> logger)
        {
            _articleLoadService = articleLoadService;
            _sourceService = sourceService;
            _logger = logger;
        }

        [HttpGet("{id:required}")]
        public async Task<IActionResult> LoadArticles(string id)
        {
            try
            {
                var source = await _sourceService.GetByIdAsync(long.Parse(id));
                if (source == null)
                {
                    _logger.LogWarning($"Source with id {id} was not found");
                    return RedirectToAction("Index", "Sources");
                }

                await _articleLoadService.LoadArticles(source);

                _logger.LogInformation($"Articles from source {source.Name} were loaded successfully");

                return RedirectToAction("Index", "Sources");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while loading articles from source. SourceId: {id}");
                return StatusCode(500);
            }
        }
    }
}
