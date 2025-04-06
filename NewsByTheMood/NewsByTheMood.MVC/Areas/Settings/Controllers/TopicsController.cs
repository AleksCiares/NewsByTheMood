using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Mappers;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Topics controller
    [Area("Settings")]
    [Route("Settings/[controller]/[action]")]
    public class TopicsController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicsController> _logger;
        private readonly TopicsMapper _topicMapper;

        public TopicsController(ITopicService topicService, ILogger<TopicsController> logger, TopicsMapper topicMapper  )
        {
            _topicService = topicService;
            _logger = logger;
            _topicMapper = topicMapper;
        }

        // Get range of topics
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
         {
            try
            {
                var totalTopics = await _topicService.CountAsync();
                var topics = Array.Empty<TopicSettingsModel>();

                if (totalTopics > 0)
                {
                    topics = (await _topicService.GetRangeAsync(
                        pagination.Page,
                        pagination.PageSize))
                        .Select(topic => _topicMapper.TopicToTopicSettingsModel(topic))
                        .ToArray();

                    _logger.LogDebug($"Topics were fetch successfully");
                }
                else
                {
                    _logger.LogDebug("No topics were found");
                }

                return View(new TopicSettingsCollectionModel()
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
            try
            {
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while preparing to create topic");
                return StatusCode(500);
            }
        }

        // Create topic item processing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TopicSettingsModel topic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(topic);
                }
                
                _logger.LogInformation($"Creating topic \"{topic.Name}\"");

                if (await _topicService.AddAsync(_topicMapper.TopicSettingsModelToTopic(topic)))
                {
                    _logger.LogInformation($"Topic \"{topic.Name}\" was created successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Topic \"{topic.Name}\" was not created");
                    return BadRequest("Something gone wrong, while creating source. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating topic {topic.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit topic item
        [HttpGet("{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var topic = await _topicService.GetByIdAsync(long.Parse(id));
                if (topic == null)
                {
                    _logger.LogWarning($"Topic id={id} was not found");
                    return BadRequest("Something gone wrong, while getting source. Watch logs to more information");
                }

                return View(new TopicSettingsEditModel()
                {
                    Topic = _topicMapper.TopicToTopicSettingsModel(topic),
                    RelatedSourceCount = topic.Sources.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update topic id={id}");
                return StatusCode(500);
            }
        }

        // Edit topic item proccessing
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Edit([FromForm] TopicSettingsEditModel topicEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(topicEdit);
                }
                
                _logger.LogInformation($"Updating topic id={topicEdit.Topic.Id}");

                if (await _topicService.UpdateAsync(_topicMapper.TopicSettingsModelToTopic(topicEdit.Topic)))
                {
                    _logger.LogInformation($"Topic id={topicEdit.Topic.Id} was updated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Topic id={topicEdit.Topic.Id} was not updated");
                    return BadRequest("Something gone wrong, while updating topic. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating topic {topicEdit.Topic.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete topic item
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
               _logger.LogInformation($"Deleting topic id={id}");

                if (await _topicService.DeleteAsync(long.Parse(id)))
                {
                    _logger.LogInformation($"Topic id={id} was deleted successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Topic id={id} was not deleted");
                    return BadRequest("Something gone wrong, while deleting topic. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting topic id={id}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> NameIsAvailable(TopicSettingsModel topic)
        {
            try
            {
                var isEXists = await _topicService.IsExistsByNameAsync(topic.Name);
                if (isEXists && !topic.Name.IsNullOrEmpty())
                {
                    var topicTemp = await _topicService.GetByIdAsync(long.Parse(topic.Id));
                    if(topicTemp != null && topicTemp.Name.Equals(topic.Name))
                    {
                        isEXists = false;
                    }
                }
                return Json(!isEXists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while checking topic name \"{topic.Name}\" availability");
                return StatusCode(500);
            }
        }
    }
}
