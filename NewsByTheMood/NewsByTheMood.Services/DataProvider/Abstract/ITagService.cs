using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    /// <summary>
    /// Interface of tags provider service
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Get tag by name
        /// </summary>
        public Task<Tag?> GetByNameAsync(string tagName);

        /// <summary>
        /// Is exist tag by name
        /// </summary>
        public Task<bool> IsExistsByNameAsync(string tagName);

        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        public Task<Tag[]> GetAllAsync();

        /// <summary>
        /// Add tag
        /// </summary>
        public Task AddAsync(Tag tag);

    }
}