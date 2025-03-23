using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Home articles controller
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ITopicService _topicService;
        private readonly ILogger<HomeController> _logger;
        private readonly short _defaultPositivity = 0;

        public HomeController(IArticleService articleService, ITopicService topicService, ILogger<HomeController> logger)
        {
            _articleService = articleService;
            _topicService = topicService;
            _logger = logger;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalArticles = await _articleService.CountAsync(_defaultPositivity);
                var articlesPreviews = Array.Empty<ArticleDisplayPreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize)) // replaced with mapper
                        .Select(article => new ArticleDisplayPreviewModel()
                        {
                            Id = article.Id.ToString(),
                            Title = article.Title,
                            PreviewImgUrl = article.PreviewImgUrl,
                            PublishDate = article.PublishDate.ToString(),
                            Positivity = article.Positivity,
                            Rating = article.Rating,
                            SourceName = article.Source.Name,
                            SourceUrl = article.Source.Url,
                            TopicName = article.Source.Topic.Name
                        })
                        .ToArray();

                    _logger.LogDebug($"Articles were fetch successfully");
                }
                else
                {
                    _logger.LogDebug("No articles were found");
                }

                return View(new ArticleDisplayPreviewCollectionModel()
                {
                    ArticlePreviews = articlesPreviews!,
                    PageInfo = new PageInfoModel()
                    {
                        Page = pagination.Page,
                        PageSize = pagination.PageSize,
                        TotalItems = totalArticles,
                    },
                    PageTitle = "Home"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting articles. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, ");
                return StatusCode(500);
            }
        }

        // Get range of articles privew by topic
        [HttpGet("topic/{id:required}")]
        public async Task<IActionResult> Topic([FromRoute] string id, [FromQuery] PaginationModel pagination)
        {
            try
            {
                var topic = await _topicService.GetByNameAsync(id);
                if (topic == null)
                {
                    _logger.LogDebug($"Topic {id} not found");
                    return BadRequest();
                }

                var totalArticles = await _articleService.CountByTopicAsync(_defaultPositivity, topic.Id);
                var articlesPreviews = Array.Empty<ArticleDisplayPreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeByTopicAsync(
                        _defaultPositivity,
                        topic.Id,
                        pagination.Page,
                        pagination.PageSize)) // replaced with mapper
                        .Select(a => new ArticleDisplayPreviewModel()
                        {
                            Id = a.Id.ToString(),
                            Title = a.Title,
                            PreviewImgUrl = a.PreviewImgUrl,
                            PublishDate = a.PublishDate.ToString(),
                            Positivity = a.Positivity,
                            Rating = a.Rating,
                            SourceName = a.Source.Name,
                            SourceUrl = a.Source.Url,
                            TopicName = a.Source.Topic.Name
                        })
                        .ToArray();

                    _logger.LogDebug($"Articles by topic {topic.Name} were fetch successfully");
                }
                else
                {
                    _logger.LogDebug($"No articles with topic {topic.Name} were found");
                }

                return View("Index", new ArticleDisplayPreviewCollectionModel()
                {
                    ArticlePreviews = articlesPreviews!,
                    PageInfo = new PageInfoModel()
                    {
                        Page = pagination.Page,
                        PageSize = pagination.PageSize,
                        TotalItems = totalArticles,
                    },
                    PageTitle = topic.Name,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting articles by topic. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, ");
                return StatusCode(500);
            }
        }

        // Get certain article
        [HttpGet("detail/{id:required}")]
        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(long.Parse(id));
                if (article == null)
                {
                    _logger.LogDebug($"Article {id} not found");
                    return BadRequest();
                }

                return View(new ArticleDisplayModel()
                {
                    Url = article.Url,
                    Title = article.Title,
                    PreviewImgUrl = article.PreviewImgUrl,
                    Body = article.Body,
                    PublishDate = article.PublishDate.ToString(),
                    Positivity = article.Positivity,
                    Rating = article.Rating,
                    SourceName = article.Source.Name,
                    TopicName = article.Source.Topic.Name,
                    ArticleTags = article.Tags.Select(t => t.Name).ToArray()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting article {id}");
                return StatusCode(500);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
