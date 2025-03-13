using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Source controller
    [Area("Settings")]
    public class SourcesController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ITopicService _topicService;

        public SourcesController(ISourceService sourceService, ITopicService topicService)
        {
            _sourceService = sourceService;
            _topicService = topicService;
        }

        // Get range of sources previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            var totalSources = await _sourceService.CountAsync();
            var sources = Array.Empty<SourcePreviewModel>();

            if (totalSources > 0)
            {
                sources = (await _sourceService.GetRangeAsync(
                    pagination.Page,
                    pagination.PageSize))
                    .Select(source => new SourcePreviewModel()
                    {
                        Id = source.Id.ToString(),
                        Name = source.Name,
                        Url = source.Url,
                        Topic = source.Topic.Name,
                    })
                    .ToArray();
            }

            return View(new SourceCollectionModel()
            {
                SourcePreviews = sources,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalSources
                }
            });
        }

        // Create source item 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new SourceCreateModel()
            {
                Topics = await GetTopicsAsync(),
            });
        }

        // Create source item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SourceCreateModel sourceCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(sourceCreate);
            }
            if (await _sourceService.IsExistsByNameAsync(sourceCreate.Source.Name))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return View(sourceCreate);
            }

            await _sourceService.AddAsync(new Source()
            {
                Name = sourceCreate.Source.Name,
                Url = sourceCreate.Source.Url,
                SurveyPeriod = sourceCreate.Source.SurveyPeriod,
                IsRandomPeriod = sourceCreate.Source.IsRandomPeriod,
                HasDynamicPage = sourceCreate.Source.HasDynamicPage,
                AcceptInsecureCerts = sourceCreate.Source.AcceptInsecureCerts,
                PageElementLoaded = sourceCreate.Source.PageElementLoaded,
                PageLoadTimeout = sourceCreate.Source.PageLoadTimeout,
                ElementLoadTimeout = sourceCreate.Source.ElementLoadTimeout,
                ArticleCollectionsPath = sourceCreate.Source.ArticleCollectionsPath,
                ArticleItemPath = sourceCreate.Source.ArticleItemPath,
                ArticleUrlPath = sourceCreate.Source.ArticleUrlPath,
                ArticleTitlePath = sourceCreate.Source.ArticleTitlePath,
                ArticlePreviewImgPath = sourceCreate.Source.ArticlePreviewImgPath,
                ArticleBodyCollectionsPath = sourceCreate.Source.ArticleBodyCollectionsPath,
                ArticleBodyItemPath = sourceCreate.Source.ArticleBodyItemPath,
                ArticlePdatePath = sourceCreate.Source.ArticlePdatePath,
                ArticleTagPath = sourceCreate.Source.ArticleTagPath,
                TopicId = long.Parse(sourceCreate.Source.TopicId),
            });

            return RedirectToAction("Index");
        }


        // Edit source item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            var source = await _sourceService.GetByIdAsync(long.Parse(id));
            if (source == null)
            {
                return BadRequest();
            }

            return View(new SourceEditModel()
            {
                Source = new SourceModel()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Url = source.Url,
                    SurveyPeriod = source.SurveyPeriod,
                    IsRandomPeriod = source.IsRandomPeriod,
                    HasDynamicPage = source.HasDynamicPage,
                    AcceptInsecureCerts = source.AcceptInsecureCerts,
                    PageElementLoaded = source.PageElementLoaded,
                    PageLoadTimeout = source.PageLoadTimeout,
                    ElementLoadTimeout = source.ElementLoadTimeout,
                    ArticleCollectionsPath = source.ArticleCollectionsPath,
                    ArticleItemPath = source.ArticleItemPath,
                    ArticleUrlPath = source.ArticleUrlPath,
                    ArticleTitlePath = source.ArticleTitlePath,
                    ArticlePreviewImgPath = source.ArticlePreviewImgPath,
                    ArticleBodyCollectionsPath = source.ArticleBodyCollectionsPath,
                    ArticleBodyItemPath = source.ArticleBodyItemPath,
                    ArticlePdatePath = source.ArticlePdatePath,
                    ArticleTagPath = source.ArticleTagPath,
                    TopicId = source.TopicId.ToString(),
                },
                Topics = await GetTopicsAsync(),
                RelatedArticlesCount = source.Articles.Count,
            });
        }

        // Edit source item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] SourceEditModel sourceEdit)
        {
            var source = await _sourceService.GetByIdAsync(long.Parse(id));
            if (source == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(sourceEdit);
            }
            if (await _sourceService.IsExistsByNameAsync(sourceEdit.Source.Name) && !source.Name.Equals(sourceEdit.Source.Name))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return View(sourceEdit);
            }

            await _sourceService.UpdateAsync(new Source()
            {
                Id = long.Parse(id),
                Name = sourceEdit.Source.Name,
                Url = sourceEdit.Source.Url,
                SurveyPeriod = sourceEdit.Source.SurveyPeriod,
                IsRandomPeriod = sourceEdit.Source.IsRandomPeriod,
                HasDynamicPage = sourceEdit.Source.HasDynamicPage,
                AcceptInsecureCerts = sourceEdit.Source.AcceptInsecureCerts,
                PageElementLoaded = sourceEdit.Source.PageElementLoaded,
                PageLoadTimeout = sourceEdit.Source.PageLoadTimeout,
                ElementLoadTimeout = sourceEdit.Source.ElementLoadTimeout,
                ArticleCollectionsPath = sourceEdit.Source.ArticleCollectionsPath,
                ArticleItemPath = sourceEdit.Source.ArticleItemPath,
                ArticleUrlPath = sourceEdit.Source.ArticleUrlPath,
                ArticleTitlePath = sourceEdit.Source.ArticleTitlePath,
                ArticlePreviewImgPath = sourceEdit.Source.ArticlePreviewImgPath,
                ArticleBodyCollectionsPath = sourceEdit.Source.ArticleBodyCollectionsPath,
                ArticleBodyItemPath = sourceEdit.Source.ArticleBodyItemPath,
                ArticlePdatePath = sourceEdit.Source.ArticlePdatePath,
                ArticleTagPath = sourceEdit.Source.ArticleTagPath,
                TopicId = long.Parse(sourceEdit.Source.TopicId),
            });

            return RedirectToAction("Index");
        }

        // Delete source utem
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id, [FromForm] SourceEditModel sourceEdit)
        {
            var source = await _sourceService.GetByIdAsync(long.Parse(id));
            if (source == null)
            {
                return BadRequest();
            }
            if (source.Articles.Count > 0)
            {
                ModelState.AddModelError("Source.Name", "The source has related articles, you cannot delete it");
                return View("Edit", sourceEdit);
            }

            await _sourceService.DeleteAsync(new Source()
            {
                Id = source.Id,
            });

            return RedirectToAction("Index");
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetTopicsAsync()
        {
            var topics = (await _topicService.GetAllAsync())
                .Select(topic => new SelectListItem()
                {
                    Value = topic.Id.ToString(),
                    Text = topic.Name,
                })
                .ToList();

            if (topics.Count == 0)
            {
                ModelState.AddModelError("Source.TopicId", "No topics have been created, to create a source you must first create a topic");
            }
           
            return topics;
        }
    }
}
