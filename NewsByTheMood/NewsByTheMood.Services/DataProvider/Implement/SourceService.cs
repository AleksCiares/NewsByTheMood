using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        // Get source preview range
        public async Task<Source[]> GetPreviewRangeAsync(int pageNumber, int pageSize)
        {
            if(pageSize <= 0 || pageNumber <= 0) return Array.Empty<Source>();

            return await this._dbContext.Sources
                .AsNoTracking()
                .Include(source => source.Topic)
                .OrderByDescending(source => source.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        // Get source count 
        public async Task<int> CountAsync()
        {
            return await this._dbContext.Sources
                .CountAsync();
        }

        // Add source item
        public async Task AddAsync(Source source)
        {
            await this._dbContext.Sources.AddAsync(source);
            await this._dbContext.SaveChangesAsync();
        }

        // Update source item
        public async Task UpdateAsync(Source source)
        {
            this._dbContext.Sources.Update(source);
            await this._dbContext.SaveChangesAsync();
        }

        // Delete source
        public async Task DeleteAsync(Source source)
        {
            this._dbContext.Sources.Remove(source);
            await this._dbContext.SaveChangesAsync();
        }

        // Check if source exist by name
        public async Task<bool> IsExistsAsync(string sourceName)
        {
            if (sourceName.IsNullOrEmpty())
            {
                return false;
            }

            return await this._dbContext.Sources
                .Where(source => source.Name == sourceName) 
                .AnyAsync();
        }

        // Get related articles
        public async Task<Article[]?> GetRelatedArticles(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            var source = await this._dbContext.Sources
                .AsNoTracking()
                .Where(source => source.Id == id)
                .Include(source => source.Topic)
                .Include(article => article.Articles)
                .SingleOrDefaultAsync();

            if (source == null)
            {
                return null;
            }

            return source.Articles.ToArray();
        }
    }
}
