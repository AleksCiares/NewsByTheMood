using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.MVC.Mappers;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Source controller
    [Area("Settings")]
    [Route("Settings/[controller]/[action]")]
    public class SourcesController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ITopicService _topicService;
        private readonly ILogger<SourcesController> _logger;
        private readonly SourcesMapper _sourceMapper;

        public SourcesController(ISourceService sourceService, ITopicService topicService, ILogger<SourcesController> logger, SourcesMapper sourceMapper)
        {
            _sourceService = sourceService;
            _topicService = topicService;
            _logger = logger;
            _sourceMapper = sourceMapper;
        }

        // Get range of sources previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalSources = await _sourceService.CountAsync();
                var sources = Array.Empty<SourceSettingsPreviewModel>();

                if (totalSources > 0)
                {
                    sources = (await _sourceService.GetRangeAsync(
                        pagination.Page,
                        pagination.PageSize))
                        .Select(source => _sourceMapper.SourceToSourceSettingPreviewModel(source))
                        .ToArray();

                    _logger.LogDebug($"Sources were fetch successfully");
                }
                else
                {
                    _logger.LogDebug("No sources were found");
                }

                return View(new SourceSettingsCollectionModel()
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching sources. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, ");
                return StatusCode(500);
            }
        }

        // Create source item 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                return View(new SourceSettingsCreateModel()
                {
                    Topics = await GetTopicsAsync(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while preparing to create source");
                return StatusCode(500);
            }
        }

        // Create source item proccessing
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SourceSettingsCreateModel sourceCreate)
        {
            try
            {
                if (!ModelState.IsValid || sourceCreate.Source == null)
                {
                    return View(sourceCreate);
                }

                _logger.LogInformation($"Creating source \"{sourceCreate.Source.Name}\"");

                if (await _sourceService.AddAsync(_sourceMapper.SourceSettingsModelToSource(sourceCreate.Source)))
                {
                    _logger.LogInformation($"Source \"{sourceCreate.Source.Name}\" was created successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Source \"{sourceCreate.Source.Name}\" was not created");
                    return BadRequest("Something gone wrong, while creating source. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, $"Error while creating source {sourceCreate.Source!.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit source item
        [HttpGet("{id:required}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var source = await _sourceService.GetByIdAsync(long.Parse(id));
                if (source == null)
                {
                    _logger.LogWarning($"Source id={id} was not found");
                    return BadRequest("Something gone wrong, while getting source. Watch logs to more information");
                }

                return View(new SourceSettingsEditModel()
                {
                    Source = _sourceMapper.SourceToSourceSettingsModel(source),
                    Topics = await GetTopicsAsync(),
                    RelatedArticlesCount = source.Articles.Count,
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update source id={id}");
                return StatusCode(500);
            }
        }

        // Edit source item proccessing
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Edit([FromForm] SourceSettingsEditModel sourceEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sourceEdit);
                }
                
                _logger.LogInformation($"Updating source id={sourceEdit.Source.Id}");

                if (await _sourceService.UpdateAsync(_sourceMapper.SourceSettingsModelToSource(sourceEdit.Source)))
                {
                    _logger.LogInformation($"Source id={sourceEdit.Source.Id} was updated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Source id={sourceEdit.Source.Id} was not updated");
                    return BadRequest("Something gone wrong, while updating source. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating source {sourceEdit.Source.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete source utem
        [HttpPost("{id:required}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                _logger.LogInformation($"Deleting source id={id}");

                if (await _sourceService.DeleteAsync(long.Parse(id)))
                {
                    _logger.LogInformation($"Source id={id} was deleted successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Source id={id} was not deleted");
                    return BadRequest("Something gone wrong, while deleting source. Watch logs to more information");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting source id={id}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> NameIsAvailable(SourceSettingsModel source)
        {
            try
            {
                var isExists = await _sourceService.IsExistsByNameAsync(source.Name);
                if (isExists && !string.IsNullOrEmpty(source.Id))
                {
                    var sourceTemp = await _sourceService.GetByIdAsync(long.Parse(source.Id));
                    if(sourceTemp != null && sourceTemp.Name.Equals(source.Name))
                    {
                        isExists = false;
                    }
                }
                return Json(!isExists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while checking if source name \"{source.Name}\" is available");
                return StatusCode(500);
            }
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetTopicsAsync()
        {
            var topics = (await _topicService.GetAllAsync())
                .Select(topic => new SelectListItem()
                {
                    Value = topic.Id.ToString(),
                    Text = topic.Name,
                })
                .ToList();

            if (topics.Count <= 0)
            {
                ModelState.AddModelError("Source.TopicId", "No topics have been created, to create a source you must first create a topic");
            }
           
            return topics;
        }
    }
}
