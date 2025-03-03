using System.Security.Cryptography;
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

        // Get range of sources previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            var totalTopics = await this._topicService.CountAsync();
            var topics = Array.Empty<TopicModel>();

            // TODO: переделать для отображения постраничного списка
            if (totalTopics > 0)
            {
                topics = (await this._topicService.GetAllAsync())
                    .Select(topic => new TopicModel()
                    { 
                        Id = topic.Id.ToString(),
                        Name = topic.Name,
                    })
                    .ToArray();
            }

            return View(new TopicCollectionModel()
            {
                Topics = topics
            });
        }

        // Add topic item
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Add topic item processing
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]TopicModel topic)
        {
            if (!ModelState.IsValid)
            {
                return View(topic);
            }

            if (await this._topicService.IsExistsAsync(topic.Name))
            {
                ModelState.AddModelError("Topic.Name", "A topic with the same name already exists");
                return View(topic);
            }

            await this._topicService.AddAsync(new Topic()
            {
                Name = topic.Name,
            });

            return View("Index");
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

            var topic = new TopicModel()
            {
                Id = topicEntity.Id.ToString(),
                Name = topicEntity.Name,
            };
            var relatedSources = (await this._topicService.GetRelatedSources(Int64.Parse(id)))!
                .Select(source => new SourcePreviewModel()
                { 
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Topic = source.Topic.Name,
                    Url = source.Url,
                    ArticleAmmount = source.Articles.Count,
                })
                .ToArray();

            return View(new TopicEditModel()
            { 
                Topic = topic,
                RelatedSources = relatedSources
            });
        }

        // Edit topic item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm]TopicEditModel topicEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(topicEdit);
            }

            if (await this._topicService.IsExistsAsync(topicEdit.Topic.Name))
            {
                ModelState.AddModelError("Topic.Name", "A topic with the same name already exists");
                return View(topicEdit);
            }

            await this._topicService.UpdateAsync(new Topic() 
            {
                Id = Int64.Parse(id),
                Name = topicEdit.Topic.Name,
            });

            return View("Index");
        }

        // Delete topic item
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute]string id, [FromForm]TopicEditModel topicEdit)
        {
            var relatedSourcesCount = (await this._topicService.GetRelatedSources(Int64.Parse(id)))?
                .Count();

            if (relatedSourcesCount > 0)
            {
                ModelState.AddModelError("RelatedSources", "There are related sources. First of all delete all related sources or change topic of it");
                return await Edit(id, topicEdit);
            }

            if (relatedSourcesCount == null)
            {
                return BadRequest();
            }

            await this._topicService.DeleteAsync(new Topic()
            {
                Id = Int64.Parse(id),
            });

            return View("Index");
        }
    }
}
