using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class SourceService : ISourceService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public SourceService(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Source?> GetByIdAsync(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            var source = await _dbContext.Sources
                .Where(source => source.Id == id)
                .SingleOrDefaultAsync();
            if (source != null)
            {
                await _dbContext.Entry(source)
                    .Reference(source => source.Topic)
                    .LoadAsync();

                await _dbContext.Entry(source)
                    .Collection(source => source.Articles)
                    .LoadAsync();


                _dbContext.Entry(source)
                    .State = EntityState.Detached;

                return source;
            }
            else
            {
                return null;
            }

            /*return await this._dbContext.Sources
                .AsNoTracking()
                .Where(source => source.Id == id)
                .Include(source => source.Topic)
                .Include(source => source.Articles)
                .SingleOrDefaultAsync();*/
        }

        public async Task<Source[]> GetRangeAsync(int pageNumber, int pageSize)
        {
            if (pageSize <= 0 || pageNumber <= 0)
            {
                return Array.Empty<Source>();
            }

            return await _dbContext.Sources
                .AsNoTracking()
                .Include(source => source.Topic)
                .Include(source => source.Articles)
                .OrderByDescending(source => source.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Source[]> GetAllAsync()
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<bool> IsExistsByNameAsync(string sourceName)
        {
            if (sourceName.IsNullOrEmpty())
            {
                return false;
            }

            return await _dbContext.Sources
                .AsNoTracking()
                .Where(source => source.Name.Equals(sourceName))
                .AnyAsync();
        }

        public async Task AddAsync(Source source)
        {
            await _dbContext.Sources.AddAsync(source);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Source source)
        {
            _dbContext.Sources.Update(source);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Source source)
        {
            _dbContext.Sources.Remove(source);
            await _dbContext.SaveChangesAsync();
        }
    }
}
