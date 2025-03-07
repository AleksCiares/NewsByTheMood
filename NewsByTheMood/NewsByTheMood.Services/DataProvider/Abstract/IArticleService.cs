using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        // Get article by certain id
        public Task<Article?> GetByIdAsync(Int64 id);

        // Get range off articles preview with certain positivity
        public Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity);

        // Get range off articles preview with certain positivity and rating
        public Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity, int rating);

        // Get range off articles preview with certain positivity and topic
        public Task<Article[]> GetRangePreviewByTopicAsync(int pageNumber, int pageSize, short positivity, Int64 topicId);

        // Get article count with certain positivity
        public Task<int> CountAsync(short positivity);

        // Get article count with certain positivity and topic
        public Task<int> CountByTopicAsync(short positivity, Int64 topicId);

        // Is exist article with current url
        public Task<bool> IsExistByUrl(string articleUrl);
    }
}
