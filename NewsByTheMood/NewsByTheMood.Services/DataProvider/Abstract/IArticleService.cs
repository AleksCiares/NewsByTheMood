using NewsByTheMood.Services.DataProvider.DTO;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    public interface IArticleService
    {
        public Task<ArticleDTO?> GetByIdAsync(Int64 id);
        //public Task<Article?> GetByIdFullPropAsync(Int64 id);
        public Task<ArticlePreviewDTO[]?> GetRangePreviewAsync(int pageSize, int pageNumber, short positivity = 0);
        public Task<ArticlePreviewDTO[]?> GetRangePreviewByTopicAsync(int pageSize, int pageNumber, string topic, short positivity = 0);

    }
}
