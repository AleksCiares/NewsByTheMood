using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of topics provider service
    public interface ITopicService
    {
        public Task<Topic[]> GetAllAsync();
        public Task<bool> IsTopicExistsAsync(string topicName);
    }
}
