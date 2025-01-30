using NewsByTheMood.Services.DataProvider.DTO;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    public interface ICommentService
    {
        public Task<CommentDTO[]?> GetRangeAsync(Int64 articleId, int pageSize, int pageNumber);
    }
}
