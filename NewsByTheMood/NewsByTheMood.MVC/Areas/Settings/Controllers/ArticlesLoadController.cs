using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Core.Settings;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.ScrapeProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Authorize(Roles = $"{AccessLevels.Admininistrator}")]
    public class ArticlesLoadController : Controller
    {
        private readonly IArticleScrapeService _articleScrapeService;
        private readonly ISourceService _sourceService;
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticlesLoadController> _logger;

        public ArticlesLoadController(
            IArticleScrapeService articleLoadService, 
            ISourceService sourceService, 
            IArticleService articleService,
            ILogger<ArticlesLoadController> logger)
        {
            _articleScrapeService = articleLoadService;
            _sourceService = sourceService;
            _articleService = articleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoadBySourceManually([FromRoute] string id)
        {
            try
            {
                var source = await _sourceService.GetByIdAsync(long.Parse(id));
                if (source == null)
                {
                    return NotFound(new 
                    {
                        GeneralErrors = "Something gone wrong. Watch logs for more information."
                    });
                }

                var result = await _articleScrapeService.ScrapeLatestBySourceAsync(source);
                if (await _articleService.AddRangeAsync(result))
                {
                    return Ok();
                }
                else
                {
                    return NotFound(new
                    {
                        GeneralErrors = "Something gone wrong. Watch logs for more information."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while loading articles from source. SourceId: {id}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{AccessLevels.Admininistrator},{AccessLevels.Editor}")]
        public async Task<IActionResult> UpdateManually([FromRoute] string id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(long.Parse(id));
                if (article == null)
                {
                    return NotFound(new
                    {
                        GeneralErrors = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }

                var source = await _sourceService.GetByIdAsync(article.SourceId);
                if (source == null)
                {
                    return NotFound(new
                    {
                        GeneralErrors = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }

                var result = await _articleScrapeService.ScrapeAsync(source, article.Url);
                result.Id = article.Id;

                if (await _articleService.UpdateAsync(result))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating article with id={id} from source.");
                return StatusCode(500);
            }
        }
    }
}
