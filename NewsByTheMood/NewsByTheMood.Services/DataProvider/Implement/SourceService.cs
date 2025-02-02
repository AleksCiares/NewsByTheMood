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
    // Service for provide Sources
    public class SourceService : ISourceService
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public SourceService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Get full properties source item
        public async Task<Source?> GetByIdAsync(Int64 id)
        {
            if(id <= 0) return null;
            return await this._dbContext.Sources
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Include(s => s.Topic)
                .FirstOrDefaultAsync();
        }

        // Get full properties source range 
        public async Task<Source[]?> GetRangeAsync(int pageSize, int pageNumber)
        {
            if(pageSize <= 0 || pageNumber <= 0) return null;
            return await this._dbContext.Sources
                .AsNoTracking()
                .Include(s => s.Topic)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }
    }
}
