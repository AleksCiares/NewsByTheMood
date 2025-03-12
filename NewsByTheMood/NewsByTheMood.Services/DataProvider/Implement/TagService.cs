using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TagService : ITagService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public TagService(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tag?> GetByNameAsync(string tagName)
        {
            if (tagName.IsNullOrEmpty())
            {
                return null;
            }

            return await _dbContext.Tags
                .AsNoTracking()
                .Where(tag => tag.Name.Equals(tagName))
                .SingleOrDefaultAsync();
        }

        public async Task<bool> IsExistsByNameAsync(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return false;
            }

            return await _dbContext.Tags
                .AsNoTracking()
                .Where(tag => tag.Name == tagName)
                .AnyAsync();
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.Tags
                .AsNoTracking()
                .Where(t => t.Name.Equals(tag.Name))
                .SingleAsync();
        }
    }
}
