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
        /*public Task<Tag?> GetByNameAsync(string tagName, CancellationToken cancellationToken = default);*/

        /// <summary>
        /// Is exist tag by name
        /// </summary>
        //public Task<bool> IsExistsByNameAsync(string tagName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Add tag
        /// </summary>
        /*public Task AddAsync(Tag tag, CancellationToken cancellationToken = default);*/

    }
}