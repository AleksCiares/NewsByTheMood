using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Articles controller
    [Area("Settings")]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly ITagService _tagService;
        private readonly ILogger<ArticlesController> _logger;
        private readonly short _defaultPositivity = 0;

        public ArticlesController(IArticleService articleService, ISourceService sourceService, ITagService tagService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _sourceService = sourceService;
            _tagService = tagService;
            _logger = logger;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalArticles = await _articleService.CountAsync(_defaultPositivity);
                var articles = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articles = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize)) // replaced with mapper
                        .Select(article => new ArticlePreviewModel()
                        {
                            Id = article.Id.ToString(),
                            Title = article.Title,
                            SourceName = article.Source.Name,
                            TopicName = article.Source.Topic.Name
                        })
                        .ToArray();

                    _logger.LogDebug($"Articles were fetch successfully");
                }
                else
                { 
                    _logger.LogDebug("No articles were found");
                }

                return View(new ArticlePreviewCollectionModel()
                {
                    ArticleShortPreviews = articles!,
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
                _logger.LogError(ex, $"Error while getting articles. " +
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
                return View(new ArticleCreateModel()
                {
                    Sources = await GetSourceAsync(),
                    Tags = await GetTagsAsync(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting create article page");
                return StatusCode(500);
            }
        }

        // Create article item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArticleCreateModel articleCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(articleCreate);
                }
                if (await _articleService.IsExistsByUrlAsync(articleCreate.Article!.Url))
                {
                    ModelState.AddModelError("Article.Url", "A article with the same url already exists");
                    return View(articleCreate);
                }

                var tags = articleCreate.Tags.Select(tag => new Tag
                {
                    Name = tag.Value
                })
                .ToList();

                await _articleService.AddAsync(new Article()
                {
                    Url = articleCreate.Article!.Url,
                    Title = articleCreate.Article!.Title,
                    PreviewImgUrl = articleCreate.Article?.PreviewImgUrl,
                    Body = articleCreate.Article?.Body,
                    PublishDate = articleCreate.Article?.PublishDate,
                    Positivity = articleCreate.Article!.Positivity,
                    Rating = articleCreate.Article!.Rating,
                    SourceId = articleCreate.Article!.SourceId,
                    Tags = tags
                });

                _logger.LogInformation($"Article {articleCreate.Article!.Title} was created successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating article");
                return StatusCode(500);
            }
        }

        // Edit article item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(long.Parse(id));
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} was not found");
                    return BadRequest();
                }

                var tags = await GetTagsAsync();
                foreach (var tag in tags)
                {
                    tag.Selected = article.Tags.Any(articleTag => articleTag.Name.Equals(tag.Value));
                }

                return View(new ArticleEditModel()
                {
                    Article = new ArticleModel()
                    {
                        Id = article.Id.ToString(),
                        Title = article.Title,
                        Url = article.Url,
                        PreviewImgUrl = article.PreviewImgUrl,
                        Body = article.Body,
                        PublishDate = article.PublishDate,
                        Positivity = article.Positivity,
                        Rating = article.Rating,
                        SourceId = article.SourceId,
                    },
                    Sources = await GetSourceAsync(),
                    Tags = tags
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting article {id}");
                return StatusCode(500);
            }
        }

        // Edit article item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] ArticleEditModel articleEdit)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(long.Parse(id));
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} was not found");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return View(articleEdit);
                }
                if (await _articleService.IsExistsByUrlAsync(articleEdit.Article.Url) && !articleEdit.Article.Url.Equals(article.Url))
                {
                    ModelState.AddModelError("Article.Url", "A url with the same url already exists");
                    articleEdit.Article.Url = article.Url;
                    return View(articleEdit);
                }

                await _articleService.UpdateAsync(new Article()
                {
                    Id = long.Parse(id),
                    Title = articleEdit.Article.Title,
                    Url = articleEdit.Article.Url,
                    PreviewImgUrl = articleEdit.Article.PreviewImgUrl,
                    Body = articleEdit.Article.Body,
                    PublishDate = articleEdit.Article.PublishDate,
                    Positivity = articleEdit.Article.Positivity,
                    Rating = articleEdit.Article.Rating,
                    SourceId = articleEdit.Article.SourceId,
                    Tags = articleEdit.Tags.Select(tag => new Tag
                    {
                        Id = Int64.Parse(tag.Value),
                        Name = tag.Text
                    })
                    .ToList()
                });

                _logger.LogInformation($"Article {articleEdit.Article.Title} was updated successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error while updating article {id}");
                return StatusCode(500);
            }
        }

        // Delete article item
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id, [FromForm] ArticleEditModel articleEdit)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(long.Parse(id));
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} was not found");
                    return BadRequest();
                }

                await _sourceService.DeleteAsync(new Source()
                {
                    Id = article.Id,
                });

                _logger.LogInformation($"Article {article.Id} was deleted successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting article {id}");
                return StatusCode(500);
            }
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetSourceAsync()
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
                ModelState.AddModelError("Article.SourceId", "No source have been created, to create a article you must first create a source");
            }

            return sources;
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetTagsAsync() // Исправить баг мульти селекта тегов
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
    }
}
