using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    /// <summary>
    /// Interface of tags provider service
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}