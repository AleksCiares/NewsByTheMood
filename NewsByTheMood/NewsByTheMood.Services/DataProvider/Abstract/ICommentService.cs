using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of comments provider service
    public interface ICommentService
    {
        // Get range of comments for certain article
        public Task<Comment[]> GetRangeAsync(Int64 articleId, int pageNumber, int pageSize);
    }
}
