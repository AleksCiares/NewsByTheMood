using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Core.Settings;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.ArticleProccessingService;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Mappers;
using NuGet.Protocol;

namespace NewsByTheMood.MVC.Areas.Settings.Controllers
{
    // Source controller
    [Area("Settings")]
    [Authorize(Roles = AccessLevels.Admininistrator)]
    public class SourcesController : Controller
    {
        private readonly ISourceService _sourceService;
        private readonly ITopicService _topicService;
        private readonly ILogger<SourcesController> _logger;
        private readonly SourcesMapper _sourceMapper;
        private readonly ArticleProccessingService _articleProccessingService;

        public SourcesController(
            ISourceService sourceService, 
            ITopicService topicService, 
            ILogger<SourcesController> logger, 
            SourcesMapper sourceMapper,
            ArticleProccessingService articleProccessingService)
        {
            _sourceService = sourceService;
            _topicService = topicService;
            _logger = logger;
            _sourceMapper = sourceMapper;
            _articleProccessingService = articleProccessingService;
        }

        // Get range of sources previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalSources = await _sourceService.CountAsync();
                var sources = Array.Empty<SourceSettingsPreviewModel>();

                if (totalSources > 0 && ItemsNotOver(pagination, totalSources))
                {
                    sources = (await _sourceService.GetRangeAsync(
                        pagination.Page,
                        pagination.PageSize))
                        .Select(source => _sourceMapper.SourceToSourceSettingPreviewModel(source))
                        .ToArray();
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
                return View(new SourceSettingsModel()
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SourceSettingsModel source)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var sourceId = await _sourceService.AddAsync(_sourceMapper.SourceSettingsModelToSource(source));
                if (sourceId > 0)
                {
                    if (source.IsActive)
                    {
                        CreateBackgroundjob(sourceId, source.SurveyPeriod);
                    }
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
               _logger.LogError(ex, $"Error while creating source {source.ToJson()}");
                return StatusCode(500);
            }
        }

        // Edit source item
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            try
            {
                var source = _sourceMapper.SourceToSourceSettingsModel(
                    await _sourceService.GetByIdAsync(long.Parse(id)));

                if (source == null)
                {
                    return BadRequest(new
                    {
                        Error = "Something gone wrong, while getting article. Watch logs to more information"
                    });
                }

                source.Topics = await GetTopicsAsync();

                return View(source);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while preparing to update source id={id}");
                return StatusCode(500);
            }
        }

        // Edit source item proccessing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] SourceSettingsModel source)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _sourceService.UpdateAsync(_sourceMapper.SourceSettingsModelToSource(source)))
                {
                    if (source.IsActive)
                    {
                        CreateBackgroundjob(long.Parse(source.Id), source.SurveyPeriod);
                    }
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
                _logger.LogError(ex, $"Error while updating source {source.ToJson()}");
                return StatusCode(500);
            }
        }

        // Delete source utem 
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                if (await _sourceService.DeleteAsync(long.Parse(id)))
                {
                    DeleteBackgroundJob(long.Parse(id));
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
                if (isExists && !string.IsNullOrEmpty(source.Id) && !source.Id.Equals("0"))
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
                ModelState.AddModelError("TopicId", "No topics have been created, to create a source you must first create a topic");
            }
           
            return topics;
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

        [NonAction]
        private void CreateBackgroundjob(long sourceId, int period, bool randomizePeriod = false)
        {

            RecurringJob.AddOrUpdate<ArticleProccessingService>(
                $"ProcessArticlesBySource-{sourceId}",
                x => x.ProcessArticlesBySource(sourceId),
                GetCronExpressionForMinutes(period));
        }

        [NonAction]
        private void DeleteBackgroundJob(long sourceId)
        {
            RecurringJob.RemoveIfExists($"ProcessArticlesBySource-{sourceId}");
        }

        [NonAction]
        private string GetCronExpressionForMinutes(int minutes)
        {
            if (minutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must be greater than 0.");
            }

            if (minutes < 60)
            {
                // Если минуты меньше 60, возвращаем Cron-строку для минут
                return $"*/{minutes} * * * *";
            }

            int hours = minutes / 60;
            int remainingMinutes = minutes % 60;

            if (minutes < 1440) // 1440 минут = 24 часа
            {
                // Если минуты больше 60, но меньше 1440, преобразуем в часы и минуты
                if (remainingMinutes == 0)
                {
                    return $"0 */{hours} * * *"; // Только часы
                }
                else
                {
                    return $"*/{remainingMinutes} */{hours} * * *"; // Часы и минуты
                }
            }

            int days = minutes / 1440;
            int remainingHours = (minutes % 1440) / 60;

            if (minutes < 10080) // 10080 минут = 7 дней
            {
                // Если минуты больше 1440, но меньше 10080, преобразуем в дни, часы и минуты
                if (remainingHours == 0 && remainingMinutes == 0)
                {
                    return $"0 0 */{days} * *"; // Только дни
                }
                else if (remainingMinutes == 0)
                {
                    return $"0 */{remainingHours} */{days} * *"; // Дни и часы
                }
                else
                {
                    return $"*/{remainingMinutes} */{remainingHours} */{days} * *"; // Дни, часы и минуты
                }
            }

            int weeks = minutes / 10080;
            int remainingDays = (minutes % 10080) / 1440;

            // Если минуты больше 10080, преобразуем в недели, дни, часы и минуты
            if (remainingDays == 0 && remainingHours == 0 && remainingMinutes == 0)
            {
                return $"0 0 * * {weeks}"; // Только недели
            }
            else if (remainingHours == 0 && remainingMinutes == 0)
            {
                return $"0 0 */{remainingDays} * {weeks}"; // Недели и дни
            }
            else if (remainingMinutes == 0)
            {
                return $"0 */{remainingHours} */{remainingDays} * {weeks}"; // Недели, дни и часы
            }
            else
            {
                return $"*/{remainingMinutes} */{remainingHours} */{remainingDays} * {weeks}"; // Недели, дни, часы и минуты
            }
        }
    }
}
