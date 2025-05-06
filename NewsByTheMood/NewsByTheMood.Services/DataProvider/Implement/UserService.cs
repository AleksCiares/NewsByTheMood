using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsByTheMood.Data;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.Services.DataProvider.Implement
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly NewsByTheMoodDbContext _dbContext;

        public UserService(UserManager<User> userManager, NewsByTheMoodDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            if (user != null)
            {
                var topics = (await _userManager.Users
                    .Include(u => u.Topics)
                    .FirstAsync(u => u.Id == user.Id))
                    .Topics;
            }

            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var topics = user.Topics.ToList();

            var existingUser = await _dbContext.Users
                .Include(u => u.Topics)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.Topics.Clear();
                await _dbContext.SaveChangesAsync();
            }

            var existingTopics = await _dbContext.Topics
                .Where(t => topics.Select(ut => ut.Id).Contains(t.Id))
                .ToListAsync();
            user.Topics = existingTopics;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    } 
}
