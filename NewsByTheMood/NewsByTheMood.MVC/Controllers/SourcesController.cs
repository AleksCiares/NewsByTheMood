using Microsoft.AspNetCore.Mvc;
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
                        ArticleAmmount = source.Articles.Count(),
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
        public IActionResult Create()
        {
            /*var topics = new List<TopicModel>();
            var topicEntities = (await this._topicService.GetAllAsync())
                .Select(topic => new TopicModel()
                {
                    Id = topic.Id.ToString(),
                    Name = topic.Name,
                    IconCssClass = topic.IconCssClass,
                })
                .ToArray();

            if (topicEntities.Length == 0)
            {
                ModelState.AddModelError(nameof(SourceCreateModel.Source.TopicId), "No topics have been created, to create a source you must first create a topic");
            }

            topics.AddRange(topicEntities);*/

            return View();
        }

        // Create source item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]SourceCreateModel sourceCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(sourceCreate);  
            }

            if (await this._sourceService.IsExistsAsync(sourceCreate.Name))
            {
                ModelState.AddModelError(nameof(sourceCreate.Name), "A source with the same name already exists");
                return View(sourceCreate);
            }

            await this._sourceService.AddAsync(new Source()
            {
                Name = sourceCreate.Name,
                Url = sourceCreate.Url,
                SurveyPeriod = sourceCreate.SurveyPeriod,
                IsRandomPeriod = sourceCreate.IsRandomPeriod,
                AcceptInsecureCerts = sourceCreate.AcceptInsecureCerts,
                PageElementLoaded = sourceCreate.PageElementLoaded,
                PageLoadTimeout = sourceCreate.PageLoadTimeout,
                ElementLoadTimeout = sourceCreate.ElementLoadTimeout,
                ArticleCollectionsPath = sourceCreate.ArticleCollectionsPath,
                ArticleItemPath = sourceCreate.ArticleItemPath,
                ArticleUrlPath = sourceCreate.ArticleUrlPath,
                ArticleTitlePath = sourceCreate.ArticleTitlePath,
                ArticlePreviewImgPath = sourceCreate.ArticlePreviewImgPath,
                ArticleBodyCollectionsPath = sourceCreate.ArticleBodyCollectionsPath,
                ArticleBodyItemPath = sourceCreate.ArticleBodyItemPath,
                ArticlePdatePath = sourceCreate.ArticlePdatePath,
                ArticleTagPath = sourceCreate.ArticleTagPath,
                TopicId = Int64.Parse(sourceCreate.TopicId),
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

            var topics = (await this._topicService.GetAllAsync())
                .Select(topic => new TopicModel()
                { 
                    Id = topic.Id.ToString(),
                    Name = topic.Name,
                    IconCssClass = topic.IconCssClass,
                })
                .ToArray();

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

            return View(new SourceEditModel()
            {
               Source = source,
               Topics = topics,
               RelatedArticles = Array.Empty<ArticlePreviewModel>()
            });
        }

        // Edit source item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm]SourceEditModel sourceEdit)
        {
            sourceEdit.Source.Id = id;
            if (!ModelState.IsValid)
            {
                return View(sourceEdit);
            }

            var sourceEntity = await this._sourceService.GetByIdAsync(Int64.Parse(sourceEdit.Source.Id));
            if (sourceEntity == null)
            {
                return BadRequest();
            }

            if (await this._sourceService.IsExistsAsync(sourceEdit.Source.Name) && !sourceEdit.Source.Name.Equals(sourceEntity.Name))
            {
                ModelState.AddModelError(nameof(sourceEdit.Source.Name), "A source with the same name already exists");
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

            /*var relatedArticlesCount = (await this._sourceService.GetRelatedArticles(Int64.Parse(id)))?
                .Count();*/

            /*            if (relatedArticlesCount > 0)
                        {
                            ModelState.AddModelError("RelatedArticles", "There are related articles. First of all delete all related articles or change source of it");
                            return await Edit(id, sourceEdit);
                        }

                        if (relatedArticlesCount == null)
                        {
                            return BadRequest();
                        }*/

            await this._sourceService.DeleteAsync(new Source()
            {
                Id = Int64.Parse(sourceEdit.Source.Id)
            });

            return RedirectToAction("Index");
        }
    }
}
