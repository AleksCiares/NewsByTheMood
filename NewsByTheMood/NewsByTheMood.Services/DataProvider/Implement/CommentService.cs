using System.Net;
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

        public async Task<int> CountByArticleIdAsync(long articleId, CancellationToken cancellationToken = default)
        {
            if (articleId <= 0)
            {
                _logger.LogWarning($"ArticleId is less than or equal to 0. ArticleId: {articleId}");
                return 0;
            }

            var result = await _mediator.Send(new GetCommentsCountByArticleIdQuery()
            {
                ArticleId = articleId,
            },
            cancellationToken);

            _logger.LogDebug($"{result} comments were found. ArticleId: {articleId}");

            return result;
        }

        public async Task<Comment?> GetByIdAsync(long commentId, CancellationToken cancellationToken = default)
        {
            if (commentId <= 0)
            {
                _logger.LogWarning($"CommentId is less than or equal to 0. CommentId: {commentId}");
                return null;
            }

            var result = await _mediator.Send(new GetCommentByIdQuery()
            {
                CommentId = commentId
            }, cancellationToken);

            if (result != null)
            {
                _logger.LogDebug($"Comment was fetched successfully. CommentId: {commentId}");
            }
            else
            {
                _logger.LogWarning($"No comment was found. CommentId: {commentId}");
            }

            return result;
        }

        public async Task<IEnumerable<Comment>> GetRangeByArticleIdAsync(long articleId, int pageNumber, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (articleId <= 0 || pageSize <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"ArticleId/PageNumber/PageSize is less than or equal to 0. " +
                    $"ArticleId: {articleId}, PageNumber: {pageNumber}, PageSize: {pageSize}");
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
                _logger.LogDebug($"{result.Count()} comments were fetch successfully. ArticleId: {articleId}, PageNumber: {pageNumber}, " +
                    $"PageSize: {pageSize}");
            }
            else
            {
                _logger.LogWarning($"No comments were found. ArticleId: {articleId}, PageNumber: {pageNumber}, " +
                    $"PageSize: {pageSize}");
            }

            return result;
        }

        public async Task<long> AddAsync(CommentCreateModel addComment, long userId, long articleId,
            CancellationToken cancellationToken = default)
        {
            if (userId <= 0 || articleId <= 0)
            {
                _logger.LogWarning($"UserId/ArticleId is less than or equal to 0. UserId: {userId}, ArticleId: {articleId}");
                return 0;
            }

            var sanitizedText = WebUtility.HtmlEncode(addComment.Text);
            var result = await _mediator.Send(new AddCommentCommand()
            {
                ArticleId = articleId,
                UserId = userId,
                Text = sanitizedText
            });

            if (result > 0)
            {
                _logger.LogDebug($"Comment was added successfully. CommentId: {result}, UserId: {userId}, " +
                    $"ArticleId: {articleId}");
            }
            else
            {
                _logger.LogError($"Failed to add comment. CommentId: {result}, UserId: {userId}, ArticleId: {articleId}, " +
                   $"Text: {sanitizedText}");
            }

            return result;
        }
    }
}
