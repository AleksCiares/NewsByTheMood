using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    // Service for provide article topics
    public class TopicService : ITopicService
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public TopicService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Topic?> GetByIdAsync(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await this._dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Id == id)
                .SingleOrDefaultAsync();
        }
        public async Task<Topic[]> GetRangeAsync(int pageNumber, int pageSize)
        {
            if (pageSize <= 0 || pageNumber <= 0) return Array.Empty<Topic>();

            return await this._dbContext.Topics
                .AsNoTracking()
                .OrderBy(topic => topic.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Topic[]> GetAllAsync()
        {
            return await this._dbContext.Topics
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<int> CountAsync()
        {
            return await this._dbContext.Topics
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<bool> IsExistsAsync(string topicName)
        {
            if (topicName.IsNullOrEmpty())
            {
                return false;
            }

            return await this._dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Name.Equals(topicName))
                .AnyAsync();
        }

        public async Task AddAsync(Topic topic)
        {
            await this._dbContext.Topics.AddAsync(topic);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Topic topic)
        {
            this._dbContext.Topics.Update(topic);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Topic topic)
        {
            this._dbContext.Topics.Remove(topic);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<Source[]?> GetRelatedSources(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            var topic = await this._dbContext.Topics
                .AsNoTracking()
                .Where(topic => topic.Id == id)
                .Include(topic => topic.Sources)
                .SingleOrDefaultAsync();

            if (topic == null)
            {
                return null;
            }

            return topic.Sources.ToArray(); ;
        }


    }
}
