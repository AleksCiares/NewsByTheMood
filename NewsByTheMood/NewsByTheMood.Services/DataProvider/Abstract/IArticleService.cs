using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        public Task<Article?> GetByIdAsync(Int64 id);
        public Task<Article[]?> GetRangePreviewAsync(int pageSize, int pageNumber, short positivity);
        public Task<Article[]?> GetRangePreviewByTopicAsync(int pageSize, int pageNumber, short positivity, string topicName);
        public Task<Article[]?> GetRangePreviewByTagAsync(int pageSize, int pageNumber, short positivity, string tagName);

    }
}
