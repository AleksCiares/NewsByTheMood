using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        // Get article count with certain positivity
        public Task<int> CountAsync(short positivity, CancellationToken cancellationToken = default);

        // Get latest articles range with certain positivity
        public Task<IEnumerable<Article>> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Get article count with certain positivity and topic
        public Task<int> CountByTopicAsync(short positivity, Int64 topicId, CancellationToken cancellationToken = default);

        // Get latest articles range with certain positivity and topic
        public Task<IEnumerable<Article>> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Get article by certain id
        public Task<Article?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default);

        // Is exist article with current url
        public Task<bool> IsExistsByUrlAsync(string articleUrl, CancellationToken cancellationToken = default);

        // Create article
        public Task<bool> AddAsync(Article article, CancellationToken cancellationToken = default);

        // Create range articles
        public Task<bool> AddRangeAsync(IEnumerable<Article> articles, CancellationToken cancellationToken = default);

        // Update article item
        public Task<bool> UpdateAsync(Article article, CancellationToken cancellationToken = default);

        // Delete article
        public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);

        // Delete range articles
        public Task<long[]> DeleteRangeAsync(long[] ids, CancellationToken cancellationToken = default);
    }
}
