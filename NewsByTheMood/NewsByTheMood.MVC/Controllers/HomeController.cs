using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.Mappers;
using NewsByTheMood.Services.MVC.Mappers;

namespace NewsByTheMood.MVC.Controllers
{
    // Home articles controller
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly ArticlesMapper _articleMapper;
        private readonly UsersMapper _usersMapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IArticleService articleService, 
            ITopicService topicService,
            IUserService userService,
            ArticlesMapper articleMapper,
            UsersMapper userMapper,
            ILogger<HomeController> logger)
        {
            _articleService = articleService;
            _topicService = topicService;
            _userService = userService;
            _articleMapper = articleMapper;
            _usersMapper = userMapper;
            _logger = logger;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                var user = await GetCurrentUserModelAsync();
                var totalArticles = await _articleService.CountAsync(user.PreferedPositivity);
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeLatestAsync(
                        user.PreferedPositivity,
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

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ArticlePreviewsPartial", articlesPreviews);
                }
                else
                {
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

                var user = await GetCurrentUserModelAsync();
                var totalArticles = await _articleService.CountByTopicAsync(user.PreferedPositivity, topic.Id);
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeByTopicAsync(
                        user.PreferedPositivity,
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

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ArticlePreviewsPartial", articlesPreviews);
                }
                else
                {
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting articles by topic. " +
                    $"Page: {pagination.Page}, " +
                    $"PageSize: {pagination.PageSize}, ");
                return StatusCode(500);
            }
        }

        // Get favorite articles
        [HttpGet("favorites")]
        [Authorize]
        public async Task<IActionResult> Favorites([FromQuery] PaginationModel pagination)
        {
            try
            {
                var user = await GetCurrentUserModelAsync();
                var totalArticles = await _articleService.CountFavoriteAsync(user.PreferedPositivity, user.TopicsIds);
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();

                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeFavoriteAsync(
                        user.PreferedPositivity,
                        user.TopicsIds,
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

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ArticlePreviewsPartial", articlesPreviews);
                }
                else
                {
                    return View("Index", new ArticlePreviewCollectionModel()
                    {
                        Articles = articlesPreviews!,
                        PageInfo = new PageInfoModel()
                        {
                            Page = pagination.Page,
                            PageSize = pagination.PageSize,
                            TotalItems = totalArticles,
                        },
                        PageTitle = "Favorites"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting favorite articles. " +
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private async Task<UserModel> GetCurrentUserModelAsync() // не подт€гивает топики
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return _usersMapper.UserToUserModel(await _userService.GetUserAsync(HttpContext.User)) ?? new UserModel();
            }

            return new UserModel();
        }
    }
}
