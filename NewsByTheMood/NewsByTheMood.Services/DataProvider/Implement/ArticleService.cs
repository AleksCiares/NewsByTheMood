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
                .Where(a => a.Id == id)
                .Include(s => s.Source)
                .Include(t => t.Source == null ? null : t.Source.Topic)
                .Include(at => at.ArticleTags)
                .ThenInclude(atn => atn.Tag)
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
                .FirstOrDefaultAsync();
        }

        // Get article with full related properties
        /*public async Task<Article?> GetByIdFullPropAsync(Int64 id)
        {
            Contract.Requires<ArgumentException>(id >= 1);
            return await this._dbContext.Articles
                .AsNoTracking()
                .Include(a => a.Source)
                .Include(t => t.ArticleTags)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(a => a.Id.Equals(id));
        }*/

        // Get range off articles preview
        public async Task<Article[]?> GetRangePreviewAsync(int pageSize, int pageNumber, short positivity)
        {
            if (pageSize <= 0 || pageNumber <= 0 || positivity <= 0) return null;
            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(a => a.Positivity >= positivity)
                .Include(a => a.Source)
                .Include(t => t.Source == null ? null : t.Source.Topic)
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                .ToArrayAsync();
        }

        // Get range off articles preview by topicId
        public async Task<Article[]?> GetRangePreviewByTopicAsync(int pageSize, int pageNumber, short positivity, string topicName)
        {
            if (pageSize <= 0 || pageNumber <= 0 || positivity <= 0 || topicName.IsNullOrEmpty()) return null;

            var topicId = await this._dbContext.Topics
                .Where(a => a.Name.Equals(topicName))
                .FirstOrDefaultAsync();
            if(topicId == null) return null;

            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(a => a.Positivity >= positivity)
                .Include(a => a.Source)
                .Include(t => t.Source == null ? null : t.Source.Topic)
                .Where( a => ( 
                a.Source != null && 
                a.Source.Topic != null && 
                a.Source.Topic.Id == topicId.Id))
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                .ToArrayAsync();
        }
    }
}
