using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Mappers;
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
        private readonly ArticleMapper _articleMapper;
        private readonly short _defaultPositivity = 0;

        public HomeController(IArticleService articleService, 
            ITopicService topicService, ILogger<HomeController> 
            logger, ArticleMapper articleMapper)
        {
            _articleService = articleService;
            _topicService = topicService;
            _logger = logger;
            _articleMapper = articleMapper;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var totalArticles = await _articleService.CountAsync(_defaultPositivity);
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeLatestAsync(
                        _defaultPositivity,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticlePreviewModel(article))
                        .ToArray();

                    _logger.LogDebug($"Articles were fetch successfully");
                }
                else
                {
                    _logger.LogDebug("No articles were found");
                }

                return View(new ArticlePreviewCollectionModel()
                {
                    Articles = articlesPreviews!,
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
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeByTopicAsync(
                        _defaultPositivity,
                        topic.Id,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticlePreviewModel(article))
                        .ToArray();

                    _logger.LogDebug($"Articles by topic {topic.Name} were fetch successfully");
                }
                else
                {
                    _logger.LogDebug($"No articles with topic {topic.Name} were found");
                }

                return View("Index", new ArticlePreviewCollectionModel()
                {
                    Articles = articlesPreviews!,
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

                return View(_articleMapper.ArticleToArticleModel(article));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting article {id}");
                return StatusCode(500);
            }
        }

        // Ajax load more articles
        [HttpGet]
        public async Task<IActionResult> LoadMore(int page, int pageSize)
        {
            try
            {
                var articles = await _articleService.GetRangeLatestAsync(_defaultPositivity, page, pageSize);
                var articlePreviews = articles.Select(article => _articleMapper.ArticleToArticlePreviewModel(article)).ToArray();

                return PartialView("_ArticlePreviewsPartial", articlePreviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading more articles");
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
