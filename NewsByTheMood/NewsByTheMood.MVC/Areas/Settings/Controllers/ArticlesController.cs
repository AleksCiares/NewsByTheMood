using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Mappers;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

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
        private readonly ArticleMapper _articleMapper;
        private readonly short _defaultPositivity = 0;

        public ArticlesController(IArticleService articleService, 
            ISourceService sourceService, ITagService tagService, 
            ILogger<ArticlesController> logger, 
            ArticleMapper articleMapper)
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
                    /*articles = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize)) // replaced with mapper
                        .Select(article => new ArticleSettingsPreviewModel()
                        {
                            Id = article.Id.ToString(),
                            Title = article.Title,
                            SourceName = article.Source.Name,
                            TopicName = article.Source.Topic.Name
                        })
                        .ToArray();*/

                    articles = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize)) // replaced with mapper
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
                return View(new ArticleSettingsCreateModel()
                {
                    Sources = await GetSourcesAsync(),
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
        public async Task<IActionResult> Create([FromForm] ArticleSettingsCreateModel articleCreate)
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

                /*await _articleService.AddAsync(new Article()
                {
                    Url = articleCreate.Article!.Url,
                    Title = articleCreate.Article!.Title,
                    PreviewImgUrl = articleCreate.Article?.PreviewImgUrl,
                    Body = articleCreate.Article?.Body,
                    PublishDate = articleCreate.Article?.PublishDate,
                    Positivity = articleCreate.Article!.Positivity,
                    Rating = articleCreate.Article!.Rating,
                    SourceId = articleCreate.Article!.SourceId,
                    Tags = articleCreate.Article.Tags.Select(tag => new Tag
                    {
                        Name = tag
                    })
                    .ToList()
                });*/


                await _articleService.AddAsync(_articleMapper.ArticleSettingsCreateModelToArticle(articleCreate));

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
        [HttpGet("{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                //var article = await _articleService.GetByIdAsync(long.Parse(id));

                var article = _articleMapper.ArticleToArticleSettingsModel(
                    await  _articleService.GetByIdAsync(long.Parse(id)));
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} was not found");
                    return BadRequest();
                }

                var tags = (await GetTagsAsync());
                tags.ForEach(tag => tag.Selected = article.Tags.Any(articleTag => articleTag.Id.ToString().Equals(tag.Value)));

                /*foreach (var tag in tags)
                {
                    tag.Selected = article.Tags.Any(articleTag => articleTag.Equals(tag.Value));
                }*/
                /*return View(new ArticleSettingsEditModel()
                {
                    Article = new ArticleSettingsModel()
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
                        IsActive = article.IsActive,
                    },
                    Sources = await GetSourceAsync(),
                    Tags = tags
                });*/

                return View(new ArticleSettingsEditModel()
                {
                    Article = article,
                    Sources = await GetSourcesAsync(),
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
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] ArticleSettingsEditModel articleEdit)
        {
            try
            {
                /*var article = await _articleService.GetByIdAsync(long.Parse(id));*/

                var article = _articleMapper.ArticleToArticleSettingsModel(
                    await _articleService.GetByIdAsync(long.Parse(id)));
                if (article == null)
                {
                    _logger.LogWarning($"Article {id} was not found");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return View(articleEdit);
                }
                if (await _articleService.IsExistsByUrlAsync(articleEdit.Article.Url) && 
                    !articleEdit.Article.Url.Equals(article.Url))
                {
                    ModelState.AddModelError("Article.Url", "A url with the same url already exists");
                    articleEdit.Article.Url = article.Url;
                    return View(articleEdit);
                }

                /*await _articleService.UpdateAsync(new Article()
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
                });*/

                articleEdit.Article!.Tags = articleEdit.Tags
                    .Where(tag => tag.Selected)
                    .Select(tag => new Tag()
                    {
                        Id = Int64.Parse(tag.Value),
                        Name = tag.Text
                    })
                    .ToArray();

                await _articleService.UpdateAsync(_articleMapper.ArticleSettingsModelToArticle(articleEdit.Article));

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
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id, [FromForm] ArticleSettingsEditModel articleEdit)
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
