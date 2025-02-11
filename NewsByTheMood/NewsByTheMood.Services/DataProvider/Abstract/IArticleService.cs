using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        public Task<Article?> GetByIdAsync(Int64 id);
        public Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity);
        public Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity, string topicName);
        public Task<int> CountAsync(short positivity);
        public Task<int> CountAsync(short positivity, string topicName);
    }
}
