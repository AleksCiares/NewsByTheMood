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
    // Service for provide comments
    public class CommentService : ICommentService
    {
        private readonly NewsByTheMoodDbContext _dbContext;
        public CommentService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Get comment for certain article loading as needed
        public async Task<Comment[]?> GetRangeAsync(Int64 articleId, int pageSize, int pageNumber)
        {
            if(articleId <= 0 || pageSize <= 0 || pageSize <= 0) return null;
            return await this._dbContext.Comments
                .AsNoTracking()
                .Where(c => c.ArticleId == articleId)
                .OrderByDescending(c => c.Position)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                /*.Select(a => new CommentDTO
                {
                    Id = a.Id,
                    Text = a.Text,
                    Position = a.Position,
                    UserName = a.User.UserName,
                    UserDisplayedName = a.User.DisplayedName,
                    AvatarUrl = a.User.AvatarUrl
                })*/
                .ToArrayAsync();
        }
    }
}
