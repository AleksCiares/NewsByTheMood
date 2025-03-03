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
                SourcePreviews = sources!,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalSources
                }
            });
        }

        // Create new item source
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var topics = (await this._topicService.GetAllAsync())
                .Select(topic => new TopicModel()
                { 
                    Id = topic.Id.ToString(),
                    Name = topic.Name,
                })
                .ToArray();

            if (topics == null || topics?.Length <= 0)
            {
                ModelState.AddModelError("Topics", "No topics have been created, to create a source you must first create a topic");
                return View();
            }

            return View(new SourceEditModel()
            {
                Topics = topics!
            });
        }

        // Create new item source proccessing
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]SourceEditModel sourceEdit) // нужна полная модель, сделать как в форме регистрации
        {
            if (!ModelState.IsValid)
            {
                return View(sourceEdit);  
            }

            if (await this._sourceService.IsExistsAsync(sourceEdit.Source.Name))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return View(sourceEdit);
            }

            await this._sourceService.AddAsync(new Source()
            {
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

            return View("Index");
        }


        // Edit certain source
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
                ArticleAmmount = sourceEntity.Articles.Count
            };

            var topics = (await this._topicService.GetAllAsync())
                .Select(t => new TopicModel()
                { 
                    Id = t.Id.ToString(),
                    Name = t.Name,
                })
                .ToArray();

            var articles = sourceEntity.Articles
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
                .ToArray();

            return View(new SourceEditModel()
            {
               Source = source,
               Topics = topics,
               RelatedArticles = articles
            });
        }

        // Edit certain source proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm] SourceEditModel sourceEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(sourceEdit);
            }

            if (await this._sourceService.IsExistsAsync(sourceEdit.Source.Name))
            {
                ModelState.AddModelError("Source.Name", "A source with the same name already exists");
                return View(sourceEdit);
            }

            var source = new Source()
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
            };

            await this._sourceService.UpdateAsync(source);
            return View("Index");
        }

        // Delete source utem
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute]string id, [FromForm]SourceEditModel sourceEdit)
        {
            var relatedArticlesCount = (await this._sourceService.GetRelatedArticles(Int64.Parse(id)))?
                .Count();

            if (relatedArticlesCount > 0)
            {
                ModelState.AddModelError("RelatedArticles", "There are related articles. First of all delete all related articles or change source of it");
                return await Edit(id, sourceEdit);
            }

            if (relatedArticlesCount == null)
            {
                return BadRequest();
            }

            await this._sourceService.DeleteAsync(new Source()
            {
                Id = Int64.Parse(id)
            });

            return View("Index");
        }
    }
}
