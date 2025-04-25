using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of comments provider service
    public interface ICommentService
    {
        // Get certain comment
        public Task<Comment?> GetByIdAsync(long commentId, CancellationToken cancellationToken = default);

        // Count of comments for certain article
        public Task<int> CountByArticleIdAsync(long articleId, CancellationToken cancellationToken = default);

        // Get range of comments for certain article
        public Task<IEnumerable<Comment>> GetRangeByArticleIdAsync(long articleId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Add comment to article
        public Task<long> AddAsync(CommentCreateModel addComment, Int64 userId, Int64 articleId, CancellationToken cancellationToken = default);
    }
}
