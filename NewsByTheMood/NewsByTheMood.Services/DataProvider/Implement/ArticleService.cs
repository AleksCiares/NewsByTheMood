using AngleSharp.Dom;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class ArticleService : IArticleService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public ArticleService(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync(short positivity)
        {
            if (positivity < 0)
            {
                return 0;
            }

            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .CountAsync();
        }

        public async Task<Article[]> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize)
        {
            if (positivity < 0 || pageNumber <= 0 || pageSize <= 0)
            {
                return Array.Empty<Article>();
            }

            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<int> CountByTopicAsync(short positivity, Int64 topicId)
        {
            if (positivity < 0 || topicId <= 0)
            {
                return 0;
            }

            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Where(article => article.Source.TopicId == topicId)
                .CountAsync();
        }

        public async Task<Article[]> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize)
        {
            if (positivity < 0 || topicId <= 0 || pageNumber <= 0 || pageSize <= 0)
            {
                return Array.Empty<Article>();
            }

            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Where(article => article.Source.TopicId == topicId)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .OrderByDescending(article => article.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Article?> GetByIdAsync(Int64 id)
        {
            if (id <= 0)
            {
                return null;
            }

            var article = await _dbContext.Articles
                .Where(article => article.Id == id)
                .SingleOrDefaultAsync();
            if (article != null)
            {
                await _dbContext.Entry(article)
                    .Reference(article => article.Source)
                    .LoadAsync();
                await _dbContext.Entry(article.Source)
                    .Reference(source => source.Topic)
                    .LoadAsync();
                await _dbContext.Entry(article)
                    .Collection(article => article.Tags)
                    .LoadAsync();

                _dbContext.Entry(article)
                    .State = EntityState.Detached;

                return article;
            }
            else
            {
                return null;
            }

            /*return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Id == id)
                .Include(article => article.Source)
                    .ThenInclude(source => source.Topic)
                .Include(article => article.Tags)
                .SingleOrDefaultAsync();*/
        }

        public async Task<bool> IsExistByUrlAsync(string articleUrl)
        {
            if (articleUrl.IsNullOrEmpty())
            {
                return false;
            }

            return await _dbContext.Articles
                .AsNoTracking()
                .Where (article => article.Url.Equals(articleUrl))
                .AnyAsync();
        }

        public async Task AddAsync(Article article)
        {
            var tagNames = article.Tags.Select(tag => tag.Name).ToList().Distinct().ToList();
            article.Tags = new();

            await _dbContext.Articles.AddAsync(article);
            foreach (var tagName in tagNames)
            {
                var tag = await _dbContext.Tags.SingleOrDefaultAsync(tag => tag.Name.Equals(tagName));
                if (tag == null)
                {
                    tag = new Tag() { Name = tagName };
                    await _dbContext.Tags.AddAsync(tag);
                }
                article.Tags.Add(tag);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(Article[] articles)
        {
            var tags = new Dictionary<string, Tag>();

            foreach (var article in articles)
            {
                var tagNames = article.Tags.Select(tag => tag.Name).ToList().Distinct().ToList();
                article.Tags = new();

                foreach (var tagName in tagNames)
                {
                    if (!tags.ContainsKey(tagName))
                    {
                        var tag = await _dbContext.Tags.SingleOrDefaultAsync(tag => tag.Name.Equals(tagName));
                        if (tag == null)
                        {
                            tag = new Tag() { Name = tagName };
                            await _dbContext.Tags.AddAsync(tag);
                        }
                        tags.Add(tagName, tag);
                    }

                    article.Tags.Add(tags[tagName]);
                }
            }

            await _dbContext.Articles.AddRangeAsync(articles);
            await _dbContext.SaveChangesAsync();
        }
    }
}
