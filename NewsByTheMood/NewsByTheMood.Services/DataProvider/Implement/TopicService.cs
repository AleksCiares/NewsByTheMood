using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TopicService : ITopicService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public TopicService(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Topic?> GetByIdAsync(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            var topic = await _dbContext.Topics
                .Where(topic => topic.Id == id)
                .SingleOrDefaultAsync();
            if (topic != null)
            {
                var sourcesTask = _dbContext.Entry(topic)
                    .Collection(topic => topic.Sources)
                    .LoadAsync();

                await sourcesTask;

                _dbContext.Entry(topic)
                    .State = EntityState.Detached;

                return topic;
            }
            else
            {
                return null;
            }

            /*return await _dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Id == id)
                .SingleOrDefaultAsync();*/
        }

        public async Task<Topic?> GetByNameAsync(string topicName)
        {
            if (topicName.IsNullOrEmpty())
            {
                return null;
            }

            return await _dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Name.Equals(topicName))
                .SingleOrDefaultAsync();
        }

        public async Task<Topic[]> GetRangeAsync(int pageNumber, int pageSize)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return Array.Empty<Topic>();
            }

            return await _dbContext.Topics
                .AsNoTracking()
                .OrderByDescending(topic => topic.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Topic[]> GetAllAsync()
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Topics
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<bool> IsExistsByNameAsync(string topicName)
        {
            if (topicName.IsNullOrEmpty())
            {
                return false;
            }

            return await _dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Name.Equals(topicName))
                .AnyAsync();
        }

        public async Task AddAsync(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Topic topic)
        {
            _dbContext.Topics.Update(topic);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Topic topic)
        {
            _dbContext.Topics.Remove(topic);
            await _dbContext.SaveChangesAsync();
        }
    }
}
