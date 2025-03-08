using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    // Service for provide articles
    public class ArticleService : IArticleService
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public ArticleService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Article?> GetByIdAsync(Int64 id)
        {
            if(id <= 0) return null;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Id == id)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .Include(article => article.Tags)
                .SingleOrDefaultAsync();
        }

        public async Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity)
        {
            if (pageNumber <= 0 || pageSize <= 0 || positivity <= 0) 
                return Array.Empty<Article>();

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity, int rating)
        {
            if (pageNumber <= 0 || pageSize <= 0 || positivity <= 0)
                return Array.Empty<Article>();

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Where(article => article.Rating >= rating)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Article[]> GetRangePreviewByTopicAsync(int pageNumber, int pageSize, short positivity, Int64 topicId)
        {
            if (pageNumber <= 0 || pageSize <= 0 || positivity <= 0 || topicId <= 0) 
                return Array.Empty<Article>();

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .Where(article => article.Source.TopicId == topicId)
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<int> CountAsync(short positivity)
        {
            if(positivity <= 0) return 0;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .CountAsync();
        }

        public async Task<int> CountByTopicAsync(short positivity, Int64 topicId)
        {
            if (positivity <= 0 || topicId <= 0) return 0;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Where(article => article.Source.TopicId == topicId)
                .CountAsync();
        }

        public async Task<bool> IsExistByUrl(string articleUrl)
        {
            if (articleUrl.IsNullOrEmpty())
            {
                return false;
            }

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where (article => article.Url == articleUrl)
                .AnyAsync();
        }

        public async Task AddAsync(Article article)
        {
            await this._dbContext.Articles.AddAsync(article);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
