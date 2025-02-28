using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of sources provider service
    public interface ISourceService
    {
        // Get source item by id
        public Task<Source?> GetByIdAsync(Int64 id);

        // Get source preview range
        public Task<Source[]> GetPreviewRangeAsync(int pageNumber, int pageSize);

        // Get source count 
        public Task<int> CountAsync();

        // Add source item
        public Task AddAsync(Source source);

        // Update source item
        public Task UpdateAsync(Source source);

        // Delete source
        public Task DeleteAsync(Source source);

        // Check if source exist by name
        public Task<bool> IsExistsAsync(string sourceName);

        // Get related articles
        public Task<Article[]?> GetRelatedArticles(Int64 id);

    }
}
