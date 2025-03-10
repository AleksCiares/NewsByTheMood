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
        private readonly short _defaultPositivity = 0;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationModel pagination)
        {
            var totalArticles = await _articleService.CountAsync(_defaultPositivity);
            var articles = Array.Empty<ArticlePreviewModel>();

            if (totalArticles > 0)
            {
                articles = (await _articleService.GetRangeLatestAsync(
                    _defaultPositivity,
                    pagination.Page,
                    pagination.PageSize)) // replaced with mapper
                    .Select(article => new ArticlePreviewModel()
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
            }

            return View(new ArticleCollectionModel()
            {
                ArticlePreviews = articles!,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalArticles,
                },
                PageTitle = "Latest"
            });
        }

        // Get range of articles privew by topic
        [HttpGet("topic/{id:required}")]
        public async Task<IActionResult> Topic([FromRoute] string id, [FromQuery] PaginationModel pagination)
        {
            var totalArticles = await _articleService.CountByTopicAsync(_defaultPositivity, long.Parse(id));
            var articles = Array.Empty<ArticlePreviewModel>();

            if (totalArticles > 0)
            {
                articles = (await _articleService.GetRangeByTopicAsync(
                    _defaultPositivity,
                    long.Parse(id),
                    pagination.Page,
                    pagination.PageSize)) // replaced with mapper
                    .Select(a => new ArticlePreviewModel()
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
            }

            return View("Index", new ArticleCollectionModel()
            {
                ArticlePreviews = articles!,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalArticles,
                },
                PageTitle = totalArticles > 0 ? articles[0].TopicName : "Oops.. nothing",
            });
        }

        // Get certain article
        [HttpGet("detail/{id:required}")]
        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            var article = await _articleService.GetByIdAsync(long.Parse(id));
            if (article == null)
            {
                return BadRequest();
            }

            return View(new ArticleModel()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
