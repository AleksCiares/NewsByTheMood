using MediatR;
using Microsoft.Extensions.Logging;
using NewsByTheMood.CQS.Commands;
using NewsByTheMood.CQS.Queries;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class CommentService : ICommentService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IMediator mediator, ILogger<CommentService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<int> CountByArticleIdAsync(Int64 articleId, CancellationToken cancellationToken = default)
        {
            if (articleId <= 0)
            {
                _logger.LogWarning($"ArticleId is less than 0. Positivity: {articleId}");
                return 0;
            }

            var result = await _mediator.Send(new GetCommentsCountByArticleIdQuery()
            {
                ArticleId = articleId,
            },
            cancellationToken);

            if (result == 0)
            {
                _logger.LogDebug($"No comments were found for article with Id: {articleId}");
            }

            return result;
        }

        public async Task<IEnumerable<Comment>> GetRangeByArticleIdAsync(Int64 articleId, int pageNumber, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (articleId <= 0 || pageSize <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"ArticleId is less than 0 or PageNumber/PageSize is less than or equal to 0. " +
                    $"Positivity: {articleId}, PageNumber: {pageNumber}, PageSize: {pageSize}");
                return Array.Empty<Comment>();
            }

            var result = await _mediator.Send(new GetCommentsRangeByArticleIdQuery()
            {
                ArticleId = articleId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

            if (result.Count() > 0)
            {
                _logger.LogDebug($"Comments for article with id={articleId} were fetch successfully");
            }
            else
            {
                _logger.LogWarning($"No comments were found for article with Id: {articleId}");
            }

            return result;
        }

        public async Task<bool> AddCommentAsync(CommentCreateModel addComment, Int64 userId, Int64 articleId,
            CancellationToken cancellationToken = default)
        {
            if (userId <= 0 || articleId <= 0)
            {
                _logger.LogWarning($"UserId/ArticleId is less than or equal to 0. UserId: {userId}, ArticleId: {articleId}");
                return false;
            }

            await _mediator.Send(new AddCommentCommand()
            {
                ArticleId = articleId,
                UserId = userId,
                Text = addComment.Text
            });

            return true;
        }
    }
}
