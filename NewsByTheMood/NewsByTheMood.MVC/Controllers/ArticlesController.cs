using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataObfuscator.Abstract;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.Controllers
{
    // Articles controller
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        // uri obfuscation
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
        public async Task<IActionResult> Index([FromQuery]PaginationModel pageData)
        {
            // validate pagemodel
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in ModelState) errors.Add(error.Key);
                return BadRequest(errors);
            }

            var articles = (await this._articleService.GetRangePreviewAsync(
                pageData.PageSize, 
                pageData.PageNumber, 
                this._articlePositivity))? // replaced with mapper
                .Select(a => new ArticlePreviewModel()
                {
                    Id = this._obfuscatorService.Obfuscate(a.Id.ToString()),
                    Title = a.Title,
                    PublishDate = a.PublishDate.ToString(),
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source?.Name,
                    TopicName = a.Source?.Topic?.Name
                })
                .ToArray();

            if(articles is null) return NotFound();
            return View(articles);
        }

        // Get range of articles privew by topic
        [HttpGet]
        public async Task<IActionResult> Topic([FromRoute]string id, [FromQuery]PaginationModel pageData)
        {
            // validate pagemodel
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in ModelState) errors.Add(error.Key);
                return BadRequest(errors);
            }
            // and add validate id

            var articles = (await this._articleService.GetRangePreviewByTopicAsync(
                pageData.PageSize, 
                pageData.PageNumber, 
                this._articlePositivity,
                id))? // replaced with mapper
                .Select(a => new ArticlePreviewModel()
                {
                    Id = this._obfuscatorService.Obfuscate(a.Id.ToString()),
                    Title = a.Title,
                    PublishDate = a.PublishDate.ToString(),
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source?.Name,
                    TopicName = a.Source?.Topic?.Name
                })
                .ToArray();

            if (articles is null) return NotFound();
            return View(articles);
        }

        // Get certain article
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute]string id)
        {
            // temp validate input variable
            Int64 tempId = 0;
            if (id.IsNullOrEmpty() || !Int64.TryParse(id, out tempId)) return BadRequest();

            var article = await this._articleService.GetByIdAsync(Int64.Parse(this._obfuscatorService.Deobfuscate(id)));
            if(article is null) return NotFound();

            var model = new ArticleModel()
            {
                Uri = article.Uri,
                Title = article.Title,
                Body = article.Body,
                PublishDate = article.PublishDate.ToString(),
                Rating = article.Rating,
                SourceName = article.Source?.Name,
                TopicName = article.Source?.Topic?.Name,
                //ArticleTags = article.ArticleTags.Select(t => t.Tag.Name).ToArray() // dont work
            };
            return View(model);
        }
    }
}
