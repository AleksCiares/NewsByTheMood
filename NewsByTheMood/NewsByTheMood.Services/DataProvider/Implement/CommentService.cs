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

        // Get range of comments for certain article
        public async Task<Comment[]> GetRangeAsync(Int64 articleId, int pageNumber, int pageSize)
        {
            if(articleId <= 0 || pageSize <= 0 || pageSize <= 0) 
                return Array.Empty<Comment>();
            
            return await this._dbContext.Comments
                .AsNoTracking()
                .Where(comment => comment.ArticleId == articleId)
                .OrderByDescending(comment => comment.Position)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
                /*.Select(a => new CommentDTO
                {
                    Id = a.Id,
                    Text = a.Text,
                    Position = a.Position,
                    UserName = a.User.UserName,
                    UserDisplayedName = a.User.DisplayedName,
                    AvatarUrl = a.User.AvatarUrl
                })*/
        }
    }
}
