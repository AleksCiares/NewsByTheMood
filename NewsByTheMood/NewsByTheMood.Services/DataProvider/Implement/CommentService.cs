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
                _logger.LogWarning($"ArticleId is less than or equal to 0. ArticleId: {articleId}. Proccess aborted.");
                return 0;
            }

            var count = await _mediator.Send(new GetCommentsCountByArticleIdQuery()
            {
                ArticleId = articleId,
            },
            cancellationToken);

            _logger.LogDebug($"{count} comments were found. ArticleId: {articleId}");

            return count;
        }

        public async Task<Comment?> GetByIdAsync(long commentId, CancellationToken cancellationToken = default)
        {
            if (commentId <= 0)
            {
                _logger.LogWarning($"CommentId is less than or equal to 0. CommentId: {commentId}. Proccess aborted.");
                return null;
            }

            var comment = await _mediator.Send(new GetCommentByIdQuery()
            {
                CommentId = commentId
            }, cancellationToken);

            if (comment != null)
            {
                _logger.LogDebug($"Comment was fetched successfully. CommentId: {commentId}");
            }
            else
            {
                _logger.LogWarning($"No comment was found. CommentId: {commentId}");
            }

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetRangeByArticleIdAsync(long articleId, int pageNumber, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            if (articleId <= 0 || pageSize <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"ArticleId/PageNumber/PageSize is less than or equal to 0. " +
                    $"ArticleId: {articleId}, PageNumber: {pageNumber}, PageSize: {pageSize}. Proccess aborted.");
                return Array.Empty<Comment>();
            }

            var comments = await _mediator.Send(new GetCommentsRangeByArticleIdQuery()
            {
                ArticleId = articleId,
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

            _logger.LogDebug($"{comments.Count()} comments were fetch. ArticleId: {articleId}, PageNumber: {pageNumber}, " +
                $"PageSize: {pageSize}");

            return comments;
        }

        public async Task<long> AddAsync(CommentCreateModel addComment, long userId, long articleId,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Adding comment... UserId: {userId}, ArticleId: {articleId}");

            if (userId <= 0 || articleId <= 0)
            {
                _logger.LogWarning($"UserId/ArticleId is less than or equal to 0. UserId: {userId}, ArticleId: {articleId}. " +
                    $"Proccess aborted.");
                return 0;
            }

            var commentId = await _mediator.Send(new AddCommentCommand()
            {
                ArticleId = articleId,
                UserId = userId,
                Text = WebUtility.HtmlEncode(addComment.Text)
            });

            if (commentId > 0)
            {
                _logger.LogInformation($"Comment was added successfully. CommentId: {commentId}, UserId: {userId}, " +
                    $"ArticleId: {articleId}");
            }
            else
            {
                _logger.LogError($"Failed to add comment. CommentId: {commentId}, UserId: {userId}, ArticleId: {articleId}");
            }

            return commentId;
        }
    }
}
