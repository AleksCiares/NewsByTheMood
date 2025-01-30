using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    public interface ITopicService
    {
        public Task<Topic[]?> GetAll();
    }
}
