using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of sources provider service
    public interface ISourceService
    {
        // Get source item by id
        public Task<Source?> GetByIdAsync(Int64 id, CancellationToken cancellationToken = default);

        // Get source range
        public Task<IEnumerable<Source>> GetRangeAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        // Get all sources
        public Task<IEnumerable<Source>> GetAllAsync(CancellationToken cancellationToken = default);

        // Get source count 
        public Task<int> CountAsync(CancellationToken cancellationToken = default);

        // Check if source exist by name
        public Task<bool> IsExistsByNameAsync(string sourceName, CancellationToken cancellationToken =default);

        // Add source item
        public Task AddAsync(Source source, CancellationToken cancellationToken = default);

        // Update source item
        public Task UpdateAsync(Source source, CancellationToken cancellationToken = default);

        // Delete source
        public Task DeleteAsync(Source source, CancellationToken cancellationToken = default);

    }
}
