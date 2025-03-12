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
        /// Add tag
        /// </summary>
        public Task<Tag> AddAsync(Tag tag);

    }
}