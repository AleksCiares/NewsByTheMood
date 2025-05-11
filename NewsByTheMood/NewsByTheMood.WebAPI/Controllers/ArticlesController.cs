using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;
using NewsByTheMood.Services.MVC.Mappers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NewsByTheMood.WebAPI.Controllers
{
    /// <summary>
    /// Articles Web API controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ArticlesMapper _articlesMapper;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(
            IArticleService articleService,
            ArticlesMapper articlesMapper,
            ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _articlesMapper = articlesMapper;
            _logger = logger;
        }

        /// <summary>
        /// Get articles collection with certain positivity
        /// </summary>
        /// <param name="positivity"></param>
        /// <param name="pagination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<ArticlePreviewModel[]>(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResult>(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetCollection(
            [FromQuery] short positivity,
            [FromQuery] PaginationModel pagination, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Validation failed",
                        details = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var totalArticles = await _articleService.CountLatestAsync(
                    positivity: positivity,
                    cancellationToken: cancellationToken);

                if (totalArticles > 0 && ItemsNotOver(pagination, totalArticles))
                {
                    var articles = (await _articleService.GetRangeLatestAsync(
                    positivity: positivity,
                    pageNumber: pagination.Page,
                    pageSize: pagination.PageSize,
                    cancellationToken: cancellationToken))
                    .Select(a => _articlesMapper.ArticleToArticlePreviewModel(a))
                    .ToArray();

                    return Ok(new
                    {
                        data = articles,
                        pagination = new
                        {
                            page = pagination.Page,
                            pageSize = pagination.PageSize,
                            totalItems = totalArticles,
                            totalPages = (int)Math.Ceiling((double)totalArticles / pagination.PageSize)
                        }
                    });
                }
                else
                {
                    return NoContent();
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the article.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Internal Server Error",
                    details = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Get article by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ArticleModel>(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var article = _articlesMapper.ArticleToArticleModel(
                    await _articleService.GetByIdAsync(
                        id: long.Parse(id), 
                        cancellationToken: cancellationToken));

                if (article == null)
                {
                    return NotFound();
                }

                return Ok(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the article.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Internal Server Error",
                    details = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="article"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType<BadRequestResult>(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)] 
        public async Task<IActionResult> Update(
            [FromBody] ArticleSettingsModel article,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (article == null)
                {
                    return BadRequest(new
                    {
                        error = "Article cannot be null",
                        details = "The request body cannot be null."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Validation failed",
                        details = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var result = await _articleService.UpdateAsync(
                    article: _articlesMapper.ArticleSettingsModelToArticle(article), 
                    cancellationToken: cancellationToken);

                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new
                    {
                        error = "Error update article",
                        details = $"Error update article with ID {article.Id}."
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the article.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Internal Server Error",
                    details = "An unexpected error occurred."
                });
            }
        }

        /// <summary>
        /// Delete article by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _articleService.IsExistsByIdAsync(
                    articleId: long.Parse(id),
                    cancellationToken: cancellationToken);

                if (!result)
                {
                    return NotFound(new
                    {
                        error = "Article not found",
                        details = $"No article found with ID {id}."
                    });
                }

                result = await _articleService.DeleteAsync(
                    id: long.Parse(id),
                    cancellationToken: cancellationToken);

                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new
                    {
                        error = "Error delete article",
                        details = $"Error delete article with ID {id}."
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the article.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Internal Server Error",
                    details = "An unexpected error occurred."
                });
            }
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
