using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Topics controller
    [Area("Settings")]
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        // Get range of topics
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            var totalTopics = await _topicService.CountAsync();
            var topics = Array.Empty<TopicModel>();

            if (totalTopics > 0)
            {
                topics = (await _topicService.GetRangeAsync(
                    pagination.Page,
                    pagination.PageSize))
                    .Select(topic => new TopicModel()
                    {
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
        public async Task<IActionResult> Create([FromForm] TopicModel topic)
        {
            if (!ModelState.IsValid)
            {
                return View(topic);
            }
            if (await _topicService.IsExistsByNameAsync(topic.Name))
            {
                ModelState.AddModelError("Name", "A topic with the same name already exists");
                return View(topic);
            }

            await _topicService.AddAsync(new Topic()
            {
                Name = topic.Name,
                IconCssClass = topic.IconCssClass,
            });

            return RedirectToAction("Index");
        }

        // Edit topic item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            var topic = await _topicService.GetByNameAsync(id);
            if (topic == null)
            {
                return BadRequest();
            }

            return View(new TopicEditModel()
            {
                Topic = new TopicModel()
                {
                    Name = topic.Name,
                    IconCssClass = topic.IconCssClass,
                },
                RelatedSourceCount = topic.Sources.Count
            });
        }

        // Edit topic item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] TopicEditModel topicEdit)
        {
            var topic = await _topicService.GetByNameAsync(id);
            if (topic == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(topicEdit);
            }
            if (await _topicService.IsExistsByNameAsync(topicEdit.Topic.Name) && !topicEdit.Topic.Name.Equals(topic.Name))
            {
                ModelState.AddModelError("Topic.Name", "A topic with the same name already exists");
                topicEdit.Topic.Name = topic.Name;  
                return View(topicEdit);
            }

            await _topicService.UpdateAsync(new Topic()
            {
                Id = topic.Id,
                Name = topicEdit.Topic.Name,
                IconCssClass = topicEdit.Topic.IconCssClass,
            });

            return RedirectToAction("Index");
        }

        // Delete topic item
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id, TopicEditModel topicEdit)
        {
            var topic = await _topicService.GetByNameAsync(id);
            if (topic == null)
            {
                return BadRequest();
            }
            if (topic.Sources.Count > 0)
            {
                ModelState.AddModelError("Topic.Name", "Can not delete topic with related source. First of all delete all related sources");
                return View("Edit", topicEdit);
            }

            await _topicService.DeleteAsync(new Topic()
            {
                Id = topic.Id,
            });

            return RedirectToAction("Index");
        }
    }
}
