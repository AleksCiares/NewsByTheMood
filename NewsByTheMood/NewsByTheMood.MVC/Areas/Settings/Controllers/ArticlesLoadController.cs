using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Core.Settings;
using NewsByTheMood.Services.ArticleProccessingService;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Authorize(Roles = $"{AccessLevels.Admininistrator}")]
    public class ArticlesLoadController : Controller
    {
        private readonly ArticleProccessingService _articleProccessingService;
        private readonly ILogger<ArticlesLoadController> _logger;

        public ArticlesLoadController(
            ArticleProccessingService articleProccessingService, 
            ILogger<ArticlesLoadController> logger)
        {
            _articleProccessingService = articleProccessingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> LoadBySourceManually([FromRoute] string id)
        {
            try
            {
                var result = await _articleProccessingService.ProcessArticlesBySourceAsync(long.Parse(id));
                if (result)
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
                var result = await _articleProccessingService.ProccessCertainArticleAsync(long.Parse(id));
                if (result)
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
