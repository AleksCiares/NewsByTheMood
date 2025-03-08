using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class TagService : ITagService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public TagService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(Tag tag)
        {
            await this._dbContext.Tags.AddAsync(tag);
            await this._dbContext.SaveChangesAsync();
        }

        public Task<Tag?> GetByName(string tagName)
        {
            if (tagName.IsNullOrEmpty())
            {
                return null;
            }

            return this._dbContext.Tags
                .AsNoTracking()
                .Where(tag => tag.Name == tagName)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return false;
            }

            return await this._dbContext.Tags
                .AsNoTracking()
                .Where(tag => tag.Name == tagName)
                .AnyAsync();
        }
    }
}
