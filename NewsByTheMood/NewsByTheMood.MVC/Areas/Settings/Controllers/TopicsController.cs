using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Core.Settings;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.MVC.Mappers;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Topics controller
    [Area("Settings")]
    [Authorize(Roles = AccessLevels.Admininistrator)]
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

                if (totalTopics > 0 && ItemsNotOver(pagination, totalTopics))
                {
                    topics = (await _topicService.GetRangeAsync(
                        pagination.Page,
                        pagination.PageSize))
                        .Select(topic => _topicMapper.TopicToTopicSettingsModel(topic))
                        .ToArray();
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
                return View(new TopicSettingsModel());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while preparing to create topic");
                return StatusCode(500);
            }
        }

        // Create topic item processing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TopicSettingsModel topic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _topicService.AddAsync(_topicMapper.TopicSettingsModelToTopic(topic)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                        "Watch logs to more information"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating topic {topic.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit topic items
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var topic = _topicMapper.TopicToTopicSettingsModel(
                    await _topicService.GetByIdAsync(long.Parse(id)));

                if (topic == null)
                {
                    return BadRequest(new
                    {
                        Error = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }

                return View(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update topic id={id}");
                return StatusCode(500);
            }
        }

        // Edit topic item proccessing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TopicSettingsModel topic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _topicService.UpdateAsync(_topicMapper.TopicSettingsModelToTopic(topic)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                        "Watch logs to more information"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating topic {topic.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete topic item
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                if (await _topicService.DeleteAsync(long.Parse(id)))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        GeneralErrors = "Something gone wrong, while creating article. " +
                            "Watch logs to more information"
                    });
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
                if (isEXists && !topic.Name.IsNullOrEmpty() && !topic.Id.Equals("0"))
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

        [NonAction]
        private bool ItemsNotOver(PaginationModel pagination, int totalItems)
        {
            var pageCount = (int)Math.Ceiling((double)totalItems / pagination.PageSize);
            if (pagination.Page <= pageCount)
            {
                return true;
            }

            return false;
        }
    }
}
