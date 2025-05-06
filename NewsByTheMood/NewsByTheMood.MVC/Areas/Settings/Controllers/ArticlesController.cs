using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Core.Settings;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Mappers;
using NewsByTheMood.Services.MVC.Mappers;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Articles controller
    [Area("Settings")]
    [Authorize(Roles = $"{AccessLevels.Admininistrator},{AccessLevels.Editor}")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ITagService _tagService;
        private readonly ICommentService _commentService;
        private readonly ILogger<ArticlesController> _logger;
        private readonly ArticlesMapper _articleMapper;
        private readonly CommentsMapper _commentMapper;
        private readonly short _defaultPositivity = 0;

        public ArticlesController(
            IArticleService articleService, 
            ISourceService sourceService, 
            ITagService tagService,
            ICommentService commentService,
            ILogger<ArticlesController> logger, 
            ArticlesMapper articleMapper,
            CommentsMapper commentMapper)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _tagService = tagService;
            _commentService = commentService;
            _logger = logger;
            _articleMapper = articleMapper;
            _commentMapper = commentMapper;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalArticles = await _articleService.CountLatestAsync(_defaultPositivity, true);
                var articles = Array.Empty<ArticleSettingsPreviewModel>();

                if (totalArticles > 0 && ItemsNotOver(pagination, totalArticles))
                {
                    articles = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize,
                        true))
                        .Select(article => _articleMapper.ArticleToArticleSettingsPreviewModel(article))
                        .ToArray();
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
                return View(new ArticleSettingsModel()
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ArticleSettingsModel article)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _articleService.AddAsync(_articleMapper.ArticleSettingsModelToArticle(article)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                        "Watch logs to more information"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating article {article.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit article item
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var article = _articleMapper.ArticleToArticleSettingsModel(
                    await  _articleService.GetByIdAsync(long.Parse(id)));

                if (article == null)
                {
                    return BadRequest(new
                    {
                        Error = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }

                article.Sources = await GetSourcesAsync();
                article.Tags = await GetTagsAsync();

                return View(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update article id={id}");
                return StatusCode(500);
            }
        }

        // Edit article item proccessing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ArticleSettingsModel article)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _articleService.UpdateAsync(_articleMapper.ArticleSettingsModelToArticle(article)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                        "Watch logs to more information"
                    });
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error while updating article {article.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete article item
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                if (await _articleService.DeleteAsync(long.Parse(id)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                            "Watch logs to more information"
                    });
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRange([FromForm] string[] ids)
        {
            try
            {
                var deletedIds = await _articleService
                    .DeleteRangeAsync(ids.Select(id => long.Parse(id)).ToArray());

                if (deletedIds.Length == ids.Length)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                            "Watch logs to more information"
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting articles {string.Join(", ", ids)}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetComments([FromRoute] string id, [FromBody] PaginationModel pagination)
        {
            try
            {
                if (!ModelState.IsValid ||
                    string.IsNullOrEmpty(id))
                {
                    return BadRequest();
                }

                var totalComments = await _commentService.CountByArticleIdAsync(
                    long.Parse(id));

                if (totalComments > 0 && ItemsNotOver(pagination, totalComments))
                {
                    var comments = (await _commentService.GetRangeByArticleIdAsync(
                        long.Parse(id),
                        pagination.Page,
                        pagination.PageSize))
                        .Select(comment => _commentMapper.CommentToCommentSettingsModel(comment))
                        .ToArray();

                    return PartialView("_CommentsPartial", comments);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting comments");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommentsRange([FromForm] string[] ids)
        {
            try
            {
                var deletedIds = await _commentService
                .DeleteRangeAsync(ids.Select(id => long.Parse(id)).ToArray());

                if (deletedIds.Length == ids.Length)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                        "Watch logs to more information"
                    });
                }
            }
            catch (Exception ex)
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
                if (isExists && !string.IsNullOrEmpty(article.Id) && !article.Id.Equals("0"))
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
                ModelState.AddModelError("SourceId", "No source have been created, " +
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
                    Value = tag.Name,
                    Text = tag.Name,
                })
                .ToList();

            return tags;
        }

        [NonAction]
        private bool ItemsNotOver(PaginationModel pagination, int totalItems)
        {
            var pageCount = (int)Math.Ceiling((double)totalItems / pagination.PageSize);
            if (pagination.Page <= pageCount)
            {
                return true;
            }

            return false;
        }
    }
}
