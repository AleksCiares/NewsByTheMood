using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.MVC.Mappers;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Articles controller
    [Area("Settings")]
    [Route("Settings/[controller]/[action]")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ITagService _tagService;
        private readonly ILogger<ArticlesController> _logger;
        private readonly ArticlesMapper _articleMapper;
        private readonly short _defaultPositivity = 0;

        public ArticlesController(IArticleService articleService, 
            ISourceService sourceService, ITagService tagService, 
            ILogger<ArticlesController> logger, 
            ArticlesMapper articleMapper)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _tagService = tagService;
            _logger = logger;
            _articleMapper = articleMapper;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalArticles = await _articleService.CountAsync(_defaultPositivity);
                var articles = Array.Empty<ArticleSettingsPreviewModel>();

                if (totalArticles > 0)
                {
                    articles = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticleSettingsPreviewModel(article))
                        .ToArray();

                    _logger.LogDebug($"Articles were fetch successfully");
                }
                else
                { 
                    _logger.LogDebug("No articles were found");
                }

                return View(new ArticleSettingsCollectionModel()
                {
                    Articles = articles!,
                    PageInfo = new PageInfoModel()
                    {
                        Page = pagination.Page,
                        PageSize = pagination.PageSize,
                        TotalItems = totalArticles,
                    },
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching articles. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, ");
                return StatusCode(500);
            }
        }

        // Create article item 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                return View(new ArticleSettingsCreateModel()
                {
                    Sources = await GetSourcesAsync(),
                    Tags = await GetTagsAsync(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while preparing to create article");
                return StatusCode(500);
            }
        }

        // Create article item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArticleSettingsCreateModel articleCreate)
        {
            try
            {
                if (!ModelState.IsValid || articleCreate.Article == null)
                {
                    return View(articleCreate);
                }

                _logger.LogInformation($"Creating article \"{articleCreate.Article!.Title}\"");

                if (await _articleService.AddAsync(_articleMapper.ArticleSettingsModelToArticle(articleCreate.Article)))
                {
                    _logger.LogInformation($"Article \"{articleCreate.Article!.Title}\" was created successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Article \"{articleCreate.Article!.Title}\" was not created");
                    return BadRequest("Something gone wrong, while creating article. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating article {articleCreate.Article!.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit article item
        [HttpGet("{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var article = _articleMapper.ArticleToArticleSettingsModel(
                    await  _articleService.GetByIdAsync(long.Parse(id)));
                if (article == null)
                {
                    _logger.LogWarning($"Article id={id} was not found");
                    return BadRequest("Something gone wrong, while getting article. Watch logs to more information");
                }

                return View(new ArticleSettingsEditModel()
                {
                    Article = article,
                    Sources = await GetSourcesAsync(),
                    Tags = await GetTagsAsync(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update article id={id}");
                return StatusCode(500);
            }
        }

        // Edit article item proccessing
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Edit([FromForm] ArticleSettingsEditModel articleEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(articleEdit);
                }

                _logger.LogInformation($"Updating article id={articleEdit.Article.Id}");

                if (await _articleService.UpdateAsync(_articleMapper.ArticleSettingsModelToArticle(articleEdit.Article)))
                {
                    _logger.LogInformation($"Article id={articleEdit.Article.Id} was updated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Article id={articleEdit.Article.Id} was not updated");
                    return BadRequest("Something gone wrong, while updating article. Watch logs to more information");
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error while updating article {articleEdit.Article.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete article item
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                _logger.LogInformation($"Deleting article id={id}");

                if (await _articleService.DeleteAsync(long.Parse(id)))
                {
                    _logger.LogInformation($"Article id={id} was deleted successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Article id={id} was not deleted");
                    return BadRequest("Something gone wrong, while deleting. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting article id={id}");
                return StatusCode(500);
            }
        }

        // Delete article range
        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] string[] ids)
        {
            try
            {
                _logger.LogInformation($"Deleting articles ids=\"{string.Join(", ", ids)}\"");

                var deletedIds = await _articleService.DeleteRangeAsync(ids.Select(id => long.Parse(id)).ToArray());

                if (deletedIds.Length == ids.Length)
                {
                    _logger.LogInformation($"Articles ids=\"{string.Join(", ", deletedIds)}\" were deleted successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Failed while deleting articles. Only {string.Join(", ", deletedIds)} were deleted successfully");
                    return BadRequest("Something gone wrong, while deleting. Watch logs to more information");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting articles {string.Join(", ", ids)}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UrlIsAvailable(ArticleSettingsModel article)
       {
            try
            {
                var isExists = await _articleService.IsExistsByUrlAsync(article.Url);
                if (isExists && !string.IsNullOrEmpty(article.Id))
                {
                    var articleTemp = await _articleService.GetByIdAsync(long.Parse(article.Id));
                    if (articleTemp != null && articleTemp.Url.Equals(article.Url))
                    {
                        isExists = false;
                    }
                }

                return Json(!isExists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while checking if article url \"{article.Url}\" is available");
                return StatusCode(500);
            }
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetSourcesAsync()
        {
            var sources = (await _sourceService.GetAllAsync())
                .Select(source => new SelectListItem()
                {
                    Value = source.Id.ToString(),
                    Text = source.Name,
                })
                .ToList();

            if (sources.Count <= 0)
            {
                ModelState.AddModelError("Article.SourceId", "No source have been created, " +
                    "to create a article you must first create a source");
            }

            return sources;
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetTagsAsync()
        {
            var tags = (await _tagService.GetAllAsync())
                .Select(tag => new SelectListItem()
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Name,
                })
                .ToList();

            return tags;
        }
    }
}
