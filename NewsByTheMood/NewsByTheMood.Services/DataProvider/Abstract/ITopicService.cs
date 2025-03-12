using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of topics provider service
    public interface ITopicService
    {
        // Get certain topic by id
        public Task<Topic?> GetByIdAsync(Int64 id);

        // Get certain topic by name
        public Task<Topic?> GetByNameAsync(string topicName);

        // Get range topics
        public Task<Topic[]> GetRangeAsync(int pageNumber, int pageSize);

        // Get all topics
        public Task<Topic[]> GetAllAsync();

        // Check if topic exist by name
        public Task<bool> IsExistsByNameAsync(string topicName);

        // Count of topics
        public Task<int> CountAsync();

        // Add topic
        public Task AddAsync(Topic topic);

        // Update topic
        public Task UpdateAsync(Topic topic);

        // Delete topic
        public Task DeleteAsync(Topic topic);
    }
}
