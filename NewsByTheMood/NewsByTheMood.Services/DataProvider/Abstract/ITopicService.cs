using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of topics provider service
    public interface ITopicService
    {
        // Get all topics
        public Task<Topic[]> GetAllAsync();

        // Get range topics
        public Task<Topic[]> GetRangeAsync(int pageNumber, int pageSize);

        // Check if topic exist by name
        public Task<bool> IsExistsAsync(string topicName);

        // Count of topics
        public Task<int> CountAsync();

        // Add topic
        public Task AddAsync(Topic topic);

        // Update topic
        public Task UpdateAsync(Topic topic);

        // Delete topic
        public Task DeleteAsync(Topic topic);

        // Get related sources
        public Task<Source[]?> GetRelatedSources(Int64 id);

        // Get certain topic by id
        public Task<Topic?> GetByIdAsync(Int64 id);
    }
}
