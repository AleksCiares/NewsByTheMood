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
        public async Task<IActionResult> Index()
        {
            var topics = Array.Empty<TopicModel>();

            if (await this._topicService.CountAsync() > 0)
            {
                topics = (await this._topicService.GetAllAsync())
                    .Select(topic => new TopicModel()
                    { 
                        Id = topic.Id.ToString(),
                        Name = topic.Name,
                        IconCssClass = topic.IconCssClass,
                    })
                    .ToArray();
            }

            return View(topics);
        }

        // Add topic item
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Add topic item processing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]TopicModel topic)
        {
            if (!ModelState.IsValid)
            {
                return View(topic);
            }

            if (await this._topicService.IsExistsAsync(topic.Name))
            {
                ModelState.AddModelError(nameof(topic.Name), "A topic with the same name already exists");
                return View(topic);
            }

            await this._topicService.AddAsync(new Topic()
            {
                Name = topic.Name,
                IconCssClass = topic.IconCssClass,
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

            /*var relatedSources = (await this._topicService.GetRelatedSources(Int64.Parse(id)))!
                .Select(source => new SourcePreviewModel()
                {
                    Id = source.Id.ToString(),
                    Name = source.Name,
                    Topic = source.Topic.Name,
                    Url = source.Url,
                    ArticleAmmount = source.Articles.Count,
                })
                .ToArray();*/

            return View(new TopicEditModel()
            { 
                Topic = new TopicModel()
                {
                    Id = id,
                    Name = topicEntity.Name,
                    IconCssClass = topicEntity.IconCssClass,
                },
                RelatedSources = Array.Empty<SourcePreviewModel>()
            });
        }

        // Edit topic item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute]string id, [FromForm]TopicModel topic)
        {
            if (!ModelState.IsValid)
            {
                topic.Id = id;
                return View(new TopicEditModel()
                {
                    Topic = topic,
                    RelatedSources = Array.Empty<SourcePreviewModel>()
                });
            }

            var topicEntity = await this._topicService.GetByIdAsync(Int64.Parse(id));
            if (topicEntity == null)
            {
                return BadRequest();
            }

            if (await this._topicService.IsExistsAsync(topic.Name) && !topic.Name.Equals(topicEntity.Name))
            {
                ModelState.AddModelError(nameof(Topic.Name), "A topic with the same name already exists");
                topic.Id = id;
                return View(new TopicEditModel()
                {
                    Topic = topic,
                    RelatedSources = Array.Empty<SourcePreviewModel>()
                });
            }

            await this._topicService.UpdateAsync(new Topic() 
            {
                Id = Int64.Parse(id),
                Name = topic.Name,
                IconCssClass = topic.IconCssClass,
            });

            return RedirectToAction("Index");
        }

        // Delete topic item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute]string id, [FromForm]TopicModel topic)
        {
            //var relatedSourcesCount = (await this._topicService.GetRelatedSources(Int64.Parse(id)))?
            //    .Count();

            //if (relatedSourcesCount > 0)
            //{
            //    ModelState.AddModelError("RelatedSources", "There are related sources. First of all delete all related sources or change topic of it");
            //    return await Edit(id, topic);
            //}

            //if (relatedSourcesCount == null)
            //{
            //    return BadRequest();
            //}

            await this._topicService.DeleteAsync(new Topic()
            {
                Id = Int64.Parse(id),
            });

            return RedirectToAction("Index");
        }
    }
}
