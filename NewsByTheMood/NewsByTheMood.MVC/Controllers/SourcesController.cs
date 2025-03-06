using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Source controller
    public class SourcesController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ITopicService _topicService;

        public SourcesController(ISourceService sourceService, ITopicService topicService)
        {
            this._sourceService = sourceService;
            this._topicService = topicService;
        }

        // Get range of sources previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            var totalSources = await this._sourceService.CountAsync();
            var sources = Array.Empty<SourcePreviewModel>();

            if (totalSources > 0)
            {
                sources = (await this._sourceService.GetPreviewRangeAsync(
                    pagination.Page,
                    pagination.PageSize))
                    .Select(source => new SourcePreviewModel()
                    {
                        Id = source.Id.ToString(),
                        Name = source.Name,
                        Url = source.Url,
                        Topic = source.Topic.Name,
                        ArticleAmmount = source.Articles == null ? 0 : source.Articles.Count,
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
                Topics = await this.GetTopicsAsync(),
            });
        }

        // Create source item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]SourceCreateModel sourceCreate)
        {
            if (!ModelState.IsValid || await this.IsSameNameExistsAsync(sourceCreate.Source.Name))
            {
                sourceCreate.Topics = await this.GetTopicsAsync();
                return View(sourceCreate);  
            }

            await this._sourceService.AddAsync(new Source()
            {
                Name = sourceCreate.Source.Name,
                Url = sourceCreate.Source.Url,
                SurveyPeriod = sourceCreate.Source.SurveyPeriod,
                IsRandomPeriod = sourceCreate.Source.IsRandomPeriod,
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
                TopicId = Int64.Parse(sourceCreate.Source.TopicId),
            });

            return RedirectToAction("Index");
        }


        // Edit source item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id)
        {
            var sourceEntity = await this._sourceService.GetByIdAsync(Int64.Parse(id));
            if (sourceEntity == null)
            {
                return BadRequest();
            }

            var source = new SourceModel()
            {
                Id = sourceEntity.Id.ToString(),
                Name = sourceEntity.Name,
                Url = sourceEntity.Url,
                SurveyPeriod = sourceEntity.SurveyPeriod,
                IsRandomPeriod = sourceEntity.IsRandomPeriod,
                AcceptInsecureCerts = sourceEntity.AcceptInsecureCerts,
                PageElementLoaded = sourceEntity.PageElementLoaded,
                PageLoadTimeout = sourceEntity.PageLoadTimeout,
                ElementLoadTimeout = sourceEntity.ElementLoadTimeout,
                ArticleCollectionsPath = sourceEntity.ArticleCollectionsPath,
                ArticleItemPath = sourceEntity.ArticleItemPath,
                ArticleUrlPath = sourceEntity.ArticleUrlPath,
                ArticleTitlePath = sourceEntity.ArticleTitlePath,
                ArticlePreviewImgPath = sourceEntity.ArticlePreviewImgPath,
                ArticleBodyCollectionsPath = sourceEntity.ArticleBodyCollectionsPath,
                ArticleBodyItemPath = sourceEntity.ArticleBodyItemPath,
                ArticlePdatePath = sourceEntity.ArticlePdatePath,
                ArticleTagPath = sourceEntity.ArticleTagPath,
                TopicId = sourceEntity.TopicId.ToString(),
            };
            var topics = await this.GetTopicsAsync();
            var relatedArticles = await this.GetRelatedArticles();  


            return View(new SourceEditModel()
            {
               Source = source,
               Topics = topics,
               RelatedArticles = relatedArticles,
            });
        }

        // Edit source item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm]SourceEditModel sourceEdit)
        {
            sourceEdit.Source.Id = id;
            if (!ModelState.IsValid || await this.IsSameNameExistsAsync(sourceEdit.Source.Id, sourceEdit.Source.Name))
            {
                sourceEdit.Topics = await this.GetTopicsAsync();
                sourceEdit.RelatedArticles = await this.GetRelatedArticles();
                return View(sourceEdit);
            }

            await this._sourceService.UpdateAsync(new Source()
            {
                Id = Int64.Parse(id),
                Name = sourceEdit.Source.Name,
                Url = sourceEdit.Source.Url,
                SurveyPeriod = sourceEdit.Source.SurveyPeriod,
                IsRandomPeriod = sourceEdit.Source.IsRandomPeriod,
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
                TopicId = Int64.Parse(sourceEdit.Source.TopicId),
            });

            return RedirectToAction("Index");
        }

        // Delete source utem
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute]string id, [FromForm]SourceEditModel sourceEdit)
        {
            sourceEdit.Source.Id = id;
            if ((await this.GetRelatedArticles()).Length > 0)
            {
                sourceEdit.Topics = await this.GetTopicsAsync();
                sourceEdit.RelatedArticles = await this.GetRelatedArticles();
                ModelState.AddModelError("Source.Name", "The source has related articles, you cannot delete it");
                return View("Edit", sourceEdit);
            }

            await this._sourceService.DeleteAsync(new Source()
            {
                Id = Int64.Parse(sourceEdit.Source.Id)
            });

            return RedirectToAction("Index");
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetTopicsAsync()
        {
            var topicEntities = (await this._topicService.GetAllAsync())
                .Select(topic => new TopicModel()
                {
                    Id = topic.Id.ToString(),
                    Name = topic.Name,
                    IconCssClass = topic.IconCssClass,
                })
                .ToArray();
            var topics = new List<SelectListItem>();
            if (topicEntities.Length == 0)
            {
                ModelState.AddModelError("Source.TopicId", "No topics have been created, to create a source you must first create a topic");
            }
            else
            {
                foreach (var topic in topicEntities)
                {
                    topics.Add(new SelectListItem()
                    {
                        Value = topic.Id,
                        Text = topic.Name
                    });
                }
            }
            return topics;
        }

        [NonAction]
        private async Task<ArticlePreviewModel[]> GetRelatedArticles()
        {


            /*            var relatedArticles = sourceEntity.Articles
                            .Select(article => new ArticlePreviewModel()
                            { 
                                Id = article.Id.ToString(),
                                Positivity = article.Positivity,
                                Rating = article.Rating,
                                SourceName = article.Source.Name,
                                TopicName = article.Source.Topic.Name,
                                PublishDate = article.PublishDate.ToString(),
                                Title = article.Title,
                            })
                            .ToArray();*/
            return Array.Empty<ArticlePreviewModel>();
        }

        [NonAction]
        private async Task<bool> IsSameNameExistsAsync(string id, string sourceName)
        {
            var sourceEntity = await this._sourceService.GetByIdAsync(Int64.Parse(id));
/*            if (sourceEntity == null)
            {
                return null;
            }*/
            if (await this._sourceService.IsExistsAsync(sourceName) && !sourceName.Equals(sourceEntity.Name))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return true;
            }

            return false;
        }

        [NonAction]
        private async Task<bool> IsSameNameExistsAsync(string sourceName)
        {
            if (await this._sourceService.IsExistsAsync(sourceName))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return true;
            }

            return false;
        }
    }
}
