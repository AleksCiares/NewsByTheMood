using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ICommentService _commentService;
        private readonly ArticlesMapper _articleMapper;
        private readonly TopicsMapper _topicMapper;
        private readonly UsersMapper _usersMapper;
        private readonly CommentsMapper _commentMapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IArticleService articleService,
            ITopicService topicService,
            IUserService userService,
            ICommentService commentService,
            ArticlesMapper articleMapper,
            TopicsMapper topicMapper,
            UsersMapper userMapper,
            CommentsMapper commentMapper,
            ILogger<HomeController> logger)
        {
            _articleService = articleService;
            _topicService = topicService;
            _userService = userService;
            _commentService = commentService;
            _articleMapper = articleMapper;
            _topicMapper = topicMapper;
            _usersMapper = userMapper;
            _commentMapper = commentMapper;
            _logger = logger;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = await GetCurrentUserModelAsync();
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();
                var totalArticles = await _articleService.CountAsync(user.PreferedPositivity);

                // TODO: check if pagination dont more that totalArticles
                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeLatestAsync(
                        user.PreferedPositivity,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticlePreviewModel(article))
                        .ToArray();
                }

                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_ArticlePreviewsPartial", articlesPreviews);
                }
                else
                {
                    return View(new ArticlePreviewCollectionModel()
                    {
                        Articles = articlesPreviews,
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
                    $"PageNumber: {pagination.Page}, " +
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
                var topic = _topicMapper.TopicToTopicSearchModel(await _topicService.GetByNameAsync(id));

                if (!ModelState.IsValid ||
                    topic == null)
                {
                    return BadRequest();
                }

                var user = await GetCurrentUserModelAsync();
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();
                var totalArticles = await _articleService.CountByTopicAsync(user.PreferedPositivity, topic.Id);

                // TODO: check if pagination dont more that totalArticles
                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeByTopicAsync(
                        user.PreferedPositivity,
                        topic.Id,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticlePreviewModel(article))
                        .ToArray();
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
                    $"PageNumber: {pagination.Page}, " +
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
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var user = await GetCurrentUserModelAsync();
                var articlesPreviews = Array.Empty<ArticlePreviewModel>();
                var totalArticles = await _articleService.CountFavoriteAsync(user.PreferedPositivity, user.TopicsIds);

                // TODO: check if pagination dont more that totalArticles
                if (totalArticles > 0)
                {
                    articlesPreviews = (await _articleService.GetRangeFavoriteAsync(
                        user.PreferedPositivity,
                        user.TopicsIds,
                        pagination.Page,
                        pagination.PageSize))
                        .Select(article => _articleMapper.ArticleToArticlePreviewModel(article))
                        .ToArray();
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
                    $"PageNumber: {pagination.Page}, " +
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
                var article = _articleMapper.ArticleToArticleModel(
                    await _articleService.GetByIdAsync(long.Parse(id)));

                if(article == null)
                {
                    return BadRequest();
                }

                return View(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting article {id}");
                return StatusCode(500);
            }
        }

        // Create comment
        [HttpPost]
        [Route("addcommenttoarticle")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddCommentToArticle([FromForm] CommentCreateModel addComment)
        {
            try
            {
                var articleId = HttpContext.Request.Headers["Referer"].ToString().Split('/').Last();

                if (!ModelState.IsValid || 
                    articleId.IsNullOrEmpty() ||
                    !await _articleService.IsExistsByIdAsync(long.Parse(articleId)))
                {
                    return BadRequest();
                }

                var result = await _commentService.AddCommentAsync( 
                    addComment,
                    Int64.Parse((await GetCurrentUserModelAsync()).Id),
                    Int64.Parse(articleId));

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while adding comment in article");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("getcomments")]
        public async Task<IActionResult> GetComments([FromBody] PaginationModel pagination)
        {
            try
            {
                var articleId = HttpContext.Request.Headers["Referer"].ToString().Split('/').Last();

                if (!ModelState.IsValid || 
                    articleId.IsNullOrEmpty() || 
                    !await _articleService.IsExistsByIdAsync(long.Parse(articleId)))
                {
                    return BadRequest();
                }

                var totalComments = await _commentService.CountByArticleIdAsync(
                    Int64.Parse(articleId));

                if (totalComments > 0 && ItemsNotOver(pagination, totalComments))
                {
                    var comments = (await _commentService.GetRangeByArticleIdAsync(
                        Int64.Parse(articleId),
                        pagination.Page,
                        pagination.PageSize))
                        .Select(comment => _commentMapper.CommentToCommentModel(comment))
                        .ToArray();

                    return PartialView("_CommentsPartial", comments);
                }
                else
                {
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting comments");
                return StatusCode(500);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private async Task<UserModel> GetCurrentUserModelAsync()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return _usersMapper.UserToUserModel(await _userService.GetUserAsync(HttpContext.User)) ?? new UserModel();
            }

            return new UserModel();
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
