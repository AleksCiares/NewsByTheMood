using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    public interface ISourceService
    {
        public Task<Source?> GetByIdFullPropAsync(Int64 id);
        public Task<Source[]?> GetRangeFullPropAsync(int pageSize, int pageNumber);
    }
}
