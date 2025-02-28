using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Get all topics
        public async Task<Topic[]> GetAllAsync()
        {
            return await this._dbContext.Topics
                .AsNoTracking()
                .ToArrayAsync();
        }

        // Check if topic exist
        public async Task<bool> IsExistsAsync(string topicName)
        {
            if (topicName.IsNullOrEmpty())
            {
                return false;
            }

            return await this._dbContext.Topics
                .Where(topic => topic.Name.Equals(topicName))
                .AnyAsync();
        }

        // Count of topics
        public async Task<int> CountAsync()
        {
            return await this._dbContext.Topics
                .CountAsync();
        }

        // Add topic
        public async Task AddAsync(Topic topic)
        {
            await this._dbContext.Topics.AddAsync(topic);
            await this._dbContext.SaveChangesAsync();
        }

        // Update topic
        public async Task UpdateAsync(Topic topic)
        {
            this._dbContext.Topics.Update(topic);
            await this._dbContext.SaveChangesAsync();
        }

        // Delete topic
        public async Task DeleteAsync(Topic topic)
        {
            this._dbContext.Topics.Remove(topic);
            await this._dbContext.SaveChangesAsync();
        }

        // Get related sources
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
                .ThenInclude(source => source.Topic)
                .Include(topic => topic.Sources)
                .ThenInclude(source => source.Articles)
                .SingleOrDefaultAsync();

            if (topic == null)
            {
                return null;
            }

            return topic.Sources.ToArray(); ;
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
    }
}
