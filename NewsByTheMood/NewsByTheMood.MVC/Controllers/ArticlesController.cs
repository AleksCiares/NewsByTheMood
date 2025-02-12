using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataObfuscator.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsByTheMood.MVC.Controllers
{
    // Articles controller
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ITopicService _topicService;
        private readonly IObfuscatorService _obfuscatorService;

        // Temp variable for article positivity
        private readonly short _articlePositivity = 1;

        public ArticlesController(IArticleService articleService, ITopicService topicService,IObfuscatorService obfuscatorService)
        {
            this._articleService = articleService;
            this._topicService = topicService;
            this._obfuscatorService = obfuscatorService;
        }

        // Get range of articles privew
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            // Validation pagination model
            if (!ModelState.IsValid) return BadRequest();

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
                        Id = this._obfuscatorService.Obfuscate(article.Id.ToString()),
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
                Title = "Articles"
            });
        }

        // Get range of articles privew by topic
        [HttpGet("{Controller}/{Action}/{topic:required:alpha}")]
        public async Task<IActionResult> Topic([FromRoute]string topic, [FromQuery]PaginationModel pagination)
        {
            // Validation pagination model
            if (!ModelState.IsValid || !await this._topicService.IsTopicExist(topic)) return BadRequest();

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
                        Id = this._obfuscatorService.Obfuscate(a.Id.ToString()),
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
                Title = topic,
            });
        }

        // Get certain article
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute]string id)
        {
            // Temp validate input variable
            Int64 tempId = 0;
            if (id.IsNullOrEmpty() || !Int64.TryParse(id, out tempId)) return BadRequest();

            var article = await this._articleService.GetByIdAsync(
                Int64.Parse(this._obfuscatorService.Deobfuscate(id)));

            if(article is null) return NotFound();

            var model = new ArticleModel()
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
            };
            return View(model);
        }
    }
}
