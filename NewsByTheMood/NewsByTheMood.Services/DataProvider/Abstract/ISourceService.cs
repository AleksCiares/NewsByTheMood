using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of sources provider service
    public interface ISourceService
    {
        // Get source item by id
        public Task<Source?> GetByIdAsync(Int64 id);

        // Get source range
        public Task<Source[]> GetRangeAsync(int pageNumber, int pageSize);

        // Get all sources
        public Task<Source[]> GetAllAsync();

        // Get source count 
        public Task<int> CountAsync();

        // Check if source exist by name
        public Task<bool> IsExistsByNameAsync(string sourceName);

        // Add source item
        public Task AddAsync(Source source);

        // Update source item
        public Task UpdateAsync(Source source);

        // Delete source
        public Task DeleteAsync(Source source);

    }
}
