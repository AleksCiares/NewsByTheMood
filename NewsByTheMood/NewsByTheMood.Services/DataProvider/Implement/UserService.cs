using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Is email exists in db
        public async Task<bool> IsEmailExists(string email)
        {
            if (email.IsNullOrEmpty()) return false;

            return await this._dbContext.Users
                .AsNoTracking()
                .Where(u => u.Email.Equals(email))
                .AnyAsync();
        }

        // Is username exists in db
        public async Task<bool> IsUserNameExists(string username)
        {
            if (username.IsNullOrEmpty()) return false;

            return await this._dbContext.Users
                .AsNoTracking()
                .Where(u => u.UserName.Equals(username))
                .AnyAsync();
        }
    }
}
