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
using NewsByTheMood.Services.DataProvider.DTO;

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
        public async Task<ArticleDTO?> GetByIdAsync(Int64 id)
        {
            Contract.Requires<ArgumentException>(id >= 1);
            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new ArticleDTO
                {
                    Id = a.Id,
                    Uri = a.Uri,
                    Title = a.Title,
                    Body = a.Body,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source == null ? null : a.Source.Name,
                    ArticleTags = a.ArticleTags.Select(a => a.Tag.Name).ToArray(),
                })
                .FirstOrDefaultAsync();
        }

        // Get article with full related properties
        //public async Task<Article?> GetByIdFullPropAsync(Int64 id)
        //{
        //    Contract.Requires<ArgumentException>(id >= 1);
        //    return await this._dbContext.Articles
        //        .AsNoTracking()
        //        .Include(a => a.Source)
        //        .Include(t => t.ArticleTags)
        //        .Include(c => c.Comments)
        //        .FirstOrDefaultAsync(a => a.Id.Equals(id));
        //}

        // Get range off articles preview
        public async Task<ArticlePreviewDTO[]?> GetRangePreviewAsync(int pageSize, int pageNumber, short positivity = 0)
        {
            Contract.Requires<ArgumentException>(pageSize >= 1 && pageNumber >= 1 && positivity >= 0);
            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(a => a.Positivity >= positivity)
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticlePreviewDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source == null ? null : a.Source.Name,
                })
                .ToArrayAsync();
        }

        // Get range off articles preview by Topic
        // will problem with getting topic from source (maybe)
        public async Task<ArticlePreviewDTO[]?> GetRangePreviewByTopicAsync(int pageSize, int pageNumber, string topic, short positivity = 0)
        {
            Contract.Requires<ArgumentException>(pageSize >= 1 && pageNumber >= 1 && topic != null && positivity >= 0);
            return await this._dbContext.Articles
                .AsNoTracking()
                .Where(a => a.Positivity >= positivity)
                .Where(a => a.Source != null && a.Source.Topic.Name.Equals(topic))
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticlePreviewDTO
                {
                    Id = a.Id,
                    PublishDate = a.PublishDate,
                    Positivity = a.Positivity,
                    Rating = a.Rating,
                    SourceName = a.Source == null ? null : a.Source.Name,
                })
                .ToArrayAsync();
        }
    }
}
