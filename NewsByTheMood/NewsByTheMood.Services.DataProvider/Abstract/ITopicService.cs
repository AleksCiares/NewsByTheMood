using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of topics provider service
    public interface ITopicService
    {
        // Get certain topic by id
        public Task<Topic?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default);

        // Get certain topic by name
        public Task<Topic?> GetByNameAsync(string topicName, CancellationToken cancellationToken = default);

        // Get range topics
        public Task<IEnumerable<Topic>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Get all topics
        public Task<IEnumerable<Topic>> GetAllAsync(CancellationToken cancellationToken = default);

        // Check if topic exist by name
        public Task<bool> IsExistsByNameAsync(string topicName, CancellationToken cancellationToken = default);

        // Count of topics
        public Task<int> CountAsync(CancellationToken cancellationToken = default);

        // Add topic
        public Task<bool> AddAsync(Topic topic, CancellationToken cancellationToken = default);

        // Update topic
        public Task<bool> UpdateAsync(Topic topic, CancellationToken cancellationToken = default);

        // Delete topic
        public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
