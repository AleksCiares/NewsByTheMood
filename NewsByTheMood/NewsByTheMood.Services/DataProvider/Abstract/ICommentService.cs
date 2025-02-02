using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of comments provider service
    public interface ICommentService
    {
        public Task<Comment[]?> GetRangeAsync(Int64 articleId, int pageSize, int pageNumber);
    }
}
