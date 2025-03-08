using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Topics controller
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        public TopicsController(ITopicService topicService) 
        {
            this._topicService = topicService;
        }

        // Get range of topics
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            var totalTopics = await _topicService.CountAsync();
            var topics = Array.Empty<TopicModel>();

            if (totalTopics > 0)
            {
                topics = (await this._topicService.GetRangeAsync(
                    pagination.Page,
                    pagination.PageSize))
                    .Select(topic => new TopicModel()
                    { 
                        Id = topic.Id.ToString(),
                        Name = topic.Name,
                        IconCssClass = topic.IconCssClass,
                    })
                    .ToArray();
            }

            return View(new TopicCollectionModel() 
            {
                Topics = topics,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalTopics
                }
            });
        }

        // Create topic item
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create topic item processing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]TopicCreateModel topicCreate)
        {
            if (!ModelState.IsValid || await this.IsSameNameExistsAsync(topicCreate.Name))
            {
                return View(topicCreate);
            }

            await this._topicService.AddAsync(new Topic()
            {
                Name = topicCreate.Name,
                IconCssClass = topicCreate.IconCssClass,
            });

            return RedirectToAction("Index");
        }

        // Edit topic item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id)
        {
            var topicEntity = await this._topicService.GetByIdAsync(Int64.Parse(id));
            if (topicEntity == null)
            {
                return BadRequest();
            }

            return View(new TopicEditModel()
            { 
                Topic = new TopicModel()
                {
                    Id = topicEntity.Id.ToString(),
                    Name = topicEntity.Name,
                    IconCssClass = topicEntity.IconCssClass,
                },
                RelatedSources = await this.GetRelatedSources()
            });
        }

        // Edit topic item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm]TopicEditModel topicEdit)
        {
            topicEdit.Topic.Id = id;
            if (!ModelState.IsValid || await this.IsSameNameExistsAsync(topicEdit.Topic.Id, topicEdit.Topic.Name))
            {
                topicEdit.RelatedSources = await this.GetRelatedSources();
                return View(topicEdit);
            }

            await this._topicService.UpdateAsync(new Topic() 
            {
                Id = Int64.Parse(topicEdit.Topic.Id),
                Name = topicEdit.Topic.Name,
                IconCssClass = topicEdit.Topic.IconCssClass,
            });

            return RedirectToAction("Index");
        }

        // Delete topic item
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute]string id, TopicEditModel topicEdit)
        {
            topicEdit.Topic.Id = id;
            if ((await this.GetRelatedSources()).Length > 0)
            {
                topicEdit.RelatedSources = await this.GetRelatedSources();
                ModelState.AddModelError("Topic.Name", "You can't delete topic with related sources");
                return View("Edit", topicEdit);
            }


            await this._topicService.DeleteAsync(new Topic()
            {
                Id = Int64.Parse(topicEdit.Topic.Id),
            });

            return RedirectToAction("Index");
        }

        [NonAction]
        private async Task<SourcePreviewModel[]> GetRelatedSources()
        {
            return Array.Empty<SourcePreviewModel>();
        }

        [NonAction]
        private async Task<bool> IsSameNameExistsAsync(string id, string sourceName)
        {
            var topicEntity = await this._topicService.GetByIdAsync(Int64.Parse(id));
            if (await this._topicService.IsExistsAsync(sourceName) && !sourceName.Equals(topicEntity.Name))
            {
                ModelState.AddModelError("Topic.Name", "A topic with the same name already exists");
                return true;
            }

            return false;
        }

        [NonAction]
        private async Task<bool> IsSameNameExistsAsync(string sourceName)
        {
            if (await this._topicService.IsExistsAsync(sourceName))
            {
                ModelState.AddModelError("Name", "A topic with the same name already exists");
                return true;
            }

            return false;
        }
    }
}
