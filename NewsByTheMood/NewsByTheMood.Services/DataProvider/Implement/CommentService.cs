using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class CommentService : ICommentService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public CommentService(NewsByTheMoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment[]> GetRangeAsync(Int64 articleId, int pageNumber, int pageSize)
        {
            if (articleId <= 0 || pageSize <= 0 || pageSize <= 0)
            {
                return Array.Empty<Comment>();
            }
            
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(comment => comment.ArticleId == articleId)
                .OrderByDescending(comment => comment.Position)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();
        }
    }
}
