using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        // Uri obfuscation
        private readonly IObfuscatorService _obfuscatorService;
        
        // Temp variable for article positivity
        private readonly short _articlePositivity = 1;

        public ArticlesController(IArticleService articleService, IObfuscatorService obfuscatorService)
        {
            this._articleService = articleService;
            this._obfuscatorService = obfuscatorService;
        }

        // Get range of articles privew
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PaginationModel pagination)
        {
            // Validation pagination model
            if (!ModelState.IsValid) return BadRequest("Bad parameters");

            var articles = (await this._articleService.GetRangePreviewAsync(
                pagination.Page,
                pagination.PageSize, 
                this._articlePositivity))? // replaced with mapper
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
            if (articles is null) return NotFound();

            var totalArticles = await this._articleService.CountAsync(this._articlePositivity);
            
            return View(new ArticleCollectionModel() 
            {
                ArticlePreviews = articles,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalArticles,
                }
            });
        }

        // Get range of articles privew by topic
        [HttpGet]
        public async Task<IActionResult> Topic([FromRoute]string id, [FromQuery]PaginationModel pagination)
        {
            // Validation pagination model
            if (!ModelState.IsValid) return BadRequest("Bad parameters");

            // And add validate id
            // here

            var articles = (await this._articleService.GetRangePreviewByTopicAsync(
                pagination.Page,
                pagination.PageSize,
                this._articlePositivity,
                id))? // replaced with mapper
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
            if (articles is null) return NotFound();

            var totalArticles = await this._articleService.CountAsync(this._articlePositivity, id);

            return View(new ArticleCollectionModel()
            {
                ArticlePreviews = articles,
                PageInfo = new PageInfoModel()
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalArticles,
                }
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
