using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NewsByTheMood.Data;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    // Service for provide users
    public class UserService : IUserService
    {
        private readonly NewsByTheMoodDbContext _dbContext;

        public UserService(NewsByTheMoodDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            if (email.IsNullOrEmpty()) return false;

            return await this._dbContext.Users
                .AsNoTracking()
                .Where(u => u.Email.Equals(email))
                .AnyAsync();
        }

        public  async Task<bool> IsUserNameExistsAsync(string username)
        {
            if (username.IsNullOrEmpty()) return false;

            return await this._dbContext.Users
                .AsNoTracking()
                .Where(u => u.UserName.Equals(username))
                .AnyAsync();
        }
    }
}
