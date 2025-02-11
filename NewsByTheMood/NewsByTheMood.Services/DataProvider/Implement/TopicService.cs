using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        // Get all article topics from db
        public async Task<Topic[]> GetAll()
        {
            return await this._dbContext.Topics
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
