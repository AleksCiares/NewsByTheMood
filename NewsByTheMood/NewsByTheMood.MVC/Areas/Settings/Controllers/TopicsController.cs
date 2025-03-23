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
        private readonly ILogger<TopicsController> _logger;

        public TopicsController(ITopicService topicService, ILogger<TopicsController> logger)
        {
            _topicService = topicService;
            _logger = logger;
        }

        // Get range of topics
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
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

                    _logger.LogDebug($"Topics were fetch successfully");
                }
                else
                {
                    _logger.LogDebug("No topics were found");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching topics. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, "); ;
                return StatusCode(500);
            }
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
            try
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

                _logger.LogInformation($"Topic {topic.Name} was created successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating topic");
                return StatusCode(500);
            }
        }

        // Edit topic item
        [HttpGet("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting topic {id}");
                return StatusCode(500);
            }
        }

        // Edit topic item proccessing
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromForm] TopicEditModel topicEdit)
        {
            try
            {
                var topic = await _topicService.GetByNameAsync(id);
                if (topic == null)
                {
                    _logger.LogWarning($"Topic with name {id} was not found");
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

                _logger.LogInformation($"Topic {topicEdit.Topic.Name} was updated successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating topic {id}");
                return StatusCode(500);
            }
        }

        // Delete topic item
        [HttpPost("{Controller}/{Action}/{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id, TopicEditModel topicEdit)
        {
            try
            {
                var topic = await _topicService.GetByNameAsync(id);
                if (topic == null)
                {
                    _logger.LogWarning($"Topic with name {id} was not found");
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

                _logger.LogInformation($"Topic {topic.Name} was deleted successfully");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting topic {id}");
                return StatusCode(500);
            }
        }
    }
}
