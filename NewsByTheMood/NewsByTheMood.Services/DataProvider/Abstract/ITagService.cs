using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    /// <summary>
    /// Interface of tags provider service
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Is exist tag by name
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Task<bool> IsExistsAsync(string tagName);

        /// <summary>
        /// Add tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Task AddAsync(Tag tag);

        /// <summary>
        /// Get tag by name
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Task<Tag?> GetByName(string tagName);
    }
}