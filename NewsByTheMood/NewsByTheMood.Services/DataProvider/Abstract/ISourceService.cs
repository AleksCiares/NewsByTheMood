using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of sources provider service
    public interface ISourceService
    {
        public Task<Source?> GetByIdAsync(Int64 id);
        public Task<Source[]?> GetRangeAsync(int pageNumber, int pageSize);
    }
}
