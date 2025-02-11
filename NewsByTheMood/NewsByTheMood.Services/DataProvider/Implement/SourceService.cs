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

        // Get source item by id
        public async Task<Source?> GetByIdAsync(Int64 id)
        {
            if(id <= 0) return null;

            return await this._dbContext.Sources
                .AsNoTracking()
                .Where(source => source.Id == id)
                .Include(source => source.Topic)
                .SingleOrDefaultAsync();
        }

        // Get source range
        public async Task<Source[]> GetRangeAsync(int pageNumber, int pageSize)
        {
            if(pageSize <= 0 || pageNumber <= 0) return Array.Empty<Source>();

            return await this._dbContext.Sources
                .AsNoTracking()
                .Include(source => source.Topic)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }
    }
}
