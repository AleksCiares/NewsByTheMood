using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Get article by certain id
        public async Task<Article?> GetByIdAsync(Int64 id)
        {
            if(id <= 0) return null;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Id == id)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .Include(article => article.ArticleTags)
                .ThenInclude(articleTags => articleTags.Tag)
                .SingleOrDefaultAsync();
                /*.Select(a => new ArticleDTO
                {
                    Id = a.Id,
                    Uri = a.Uri,
                    Title = a.Title,
                    Body = a.Body,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source == null ? null : a.Source.Name,
                    TopicName = a.Source == null ? null : a.Source.Topic == null ? null : a.Source.Topic.Name,
                    ArticleTags = a.ArticleTags.Select(a => a.Tag.Name).ToArray(),
                })*/
        }

        // Get range off articles preview with certain positivity
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
                 /*.Select(a => new ArticlePreviewDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source == null ? null : a.Source.Name,
                    TopicName = a.Source == null ? null : a.Source.Topic == null ? null : a.Source.Topic.Name 
                })*/
        }

        // Get range off articles preview with certain positivity and topic
        public async Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity, string topicName)
        {
            if (pageNumber <= 0 || pageSize <= 0 || positivity <= 0 || topicName.IsNullOrEmpty()) 
                return Array.Empty<Article>();

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Include(article => article.Source)
                .ThenInclude(source => source.Topic)
                .Where(article => article.Source.Topic.Name.Equals(topicName))
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
                /*.Select(a => new ArticlePreviewDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source!.Name,
                    TopicName = a.Source!.Topic!.Name
                })*/
        }

        //Get article count with certain positivity
        public async Task<int> CountAsync(short positivity)
        {
            if(positivity <= 0) return 0;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .CountAsync();
        }

        //Get article count with certain positivity and topic
        public async Task<int> CountAsync(short positivity, string topicName)
        {
            if (positivity <= 0 || topicName.IsNullOrEmpty()) return 0;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(article => article.Positivity >= positivity)
                .Where(article => article.Source.Topic.Name.Equals(topicName))
                .CountAsync();
        }

        // Get range off articles preview by tagName
        /*public async Task<Article[]?> GetRangePreviewByTagAsync(int pageSize, int pageNumber, short positivity, string tagName)
        {
            if (pageSize <= 0 || pageNumber <= 0 || positivity <= 0 || tagName.IsNullOrEmpty()) return null;

            return await this._dbContext.ArticleTags
                .AsNoTracking()
                .Where(t => t.Tag.Name.Equals(tagName))
                .Select(a => new Article
                {
                    Id = a.Article.Id,
                    Uri = a.Article.Uri,
                    Title = a.Article.Title,
                    Body = a.Article.Body,
                    PublishDate = a.Article.PublishDate,
                    Positivity = a.Article.Positivity,
                    Rating = a.Article.Rating,
                    Source = a.Article.Source,
                })
                .ToArrayAsync();
        }*/
    }
}
