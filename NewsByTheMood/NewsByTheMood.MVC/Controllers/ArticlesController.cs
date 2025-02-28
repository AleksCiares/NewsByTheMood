using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Filters;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Articles controller
    [ValidateModelFilter]
    [TypeFilter(typeof(SpoofModelPropertyFilter), Arguments = new object[] { "ArticlePreviews.Id" })]
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        // Temp variable for article positivity
        private readonly short _articlePositivity = 1;

        public ArticlesController(IArticleService articleService)
        {
            this._articleService = articleService;
        }

        // Get range of articles previews
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            var totalArticles = await this._articleService.CountAsync(this._articlePositivity);
            var articles = Array.Empty<ArticlePreviewModel>();

            if (totalArticles > 0)
            {
                articles = (await this._articleService.GetRangePreviewAsync(
                    pagination.Page,
                    pagination.PageSize,
                    this._articlePositivity))? // replaced with mapper
                    .Select(article => new ArticlePreviewModel()
                    {
                        Id = article.Id.ToString(),
                        Title = article.Title,
                        PublishDate = article.PublishDate.ToString(),
                        Positivity = article.Positivity,
                        Rating = article.Rating,
                        SourceName = article.Source.Name,
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
                PageTitle = "Articles"
            });
        }

        // Get range of articles privew by topic
        [HttpGet("{Controller}/{Action}/{topic:required:alpha}")]
        public async Task<IActionResult> Topic([FromRoute]string topic, [FromQuery]PaginationModel pagination)
        {
            var totalArticles = await this._articleService.CountAsync(this._articlePositivity, topic);
            var articles = Array.Empty<ArticlePreviewModel>();

            if (totalArticles > 0)
            {
                articles = (await this._articleService.GetRangePreviewAsync(
                    pagination.Page,
                    pagination.PageSize,
                    this._articlePositivity,
                    topic))? // replaced with mapper
                    .Select(a => new ArticlePreviewModel()
                    {
                        Id = a.Id.ToString(),
                        Title = a.Title,
                        PublishDate = a.PublishDate.ToString(),
                        Positivity = a.Positivity,
                        Rating = a.Rating,
                        SourceName = a.Source.Name,
                        TopicName = a.Source.Topic.Name
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
                PageTitle = totalArticles > 0 ? topic : "",
            });
        }

        // Get certain article
        [HttpGet("{Controller}/{Action}/{id:required}")]
        [ValidateStringToIntQueryParameterFilter(nameof(id))]
        public async Task<IActionResult> Details([FromRoute]string id)
        {
            var article = await this._articleService.GetByIdAsync(Int64.Parse(id));
            if (article == null)
            {
                return BadRequest();
            } 

            return View(new ArticleModel()
            {
                Uri = article.Uri,
                Title = article.Title,
                Body = article.Body,
                PublishDate = article.PublishDate.ToString(),
                Positivity = article.Positivity,
                Rating = article.Rating,
                SourceName = article.Source.Name,
                TopicName = article.Source.Topic.Name,
                ArticleTags = article.ArticleTags.Select(t => t.Tag.Name).ToArray()
            });
        }
    }
}
