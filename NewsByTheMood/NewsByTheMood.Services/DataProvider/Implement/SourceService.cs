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
        public async Task<Source?> GetByIdFullPropAsync(Int64 id)
        {
            Contract.Requires<ArgumentException>(id >= 1);
            return await this._dbContext.Sources
                .AsNoTracking()
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        // Get full properties source range 
        public async Task<Source[]?> GetRangeFullPropAsync(int pageSize, int pageNumber)
        {
            Contract.Requires<ArgumentException>(pageSize >= 1 && pageNumber >= 1);
            return await this._dbContext.Sources
                .AsNoTracking()
                .Include(s => s.Topic)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }
    }
}
