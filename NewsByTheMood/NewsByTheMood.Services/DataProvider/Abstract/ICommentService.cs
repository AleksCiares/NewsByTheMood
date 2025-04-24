using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of comments provider service
    public interface ICommentService
    {
        // Count of comments for certain article
        public Task<int> CountByArticleIdAsync(Int64 articleId, CancellationToken cancellationToken = default);

        // Get range of comments for certain article
        public Task<IEnumerable<Comment>> GetRangeByArticleIdAsync(Int64 articleId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Add comment to article
        public Task<bool> AddCommentAsync(CommentCreateModel addComment, Int64 userId, Int64 articleId, CancellationToken cancellationToken = default);
    }
}
