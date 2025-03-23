using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        // Get article count with certain positivity
        public Task<int> CountAsync(short positivity);

        // Get latest articles range with certain positivity
        public Task<Article[]> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize);

        // Get article count with certain positivity and topic
        public Task<int> CountByTopicAsync(short positivity, Int64 topicId);

        // Get latest articles range with certain positivity and topic
        public Task<Article[]> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize);

        // Get article by certain id
        public Task<Article?> GetByIdAsync(Int64 id);

        // Is exist article with current url
        public Task<bool> IsExistsByUrlAsync(string articleUrl);

        // Create article
        public Task AddAsync(Article article);

        // Create range articles
        public Task AddRangeAsync(Article[] articles);

        // Update article item
        public Task UpdateAsync(Article article);

        // Delete article
        public Task DeleteAsync(Article article);
    }
}
