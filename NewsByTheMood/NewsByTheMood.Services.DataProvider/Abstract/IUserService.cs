using System.Security.Claims;
using NewsByTheMood.Data.Entities;


namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of users provider service
    public interface IUserService
    {
        public Task<User?> GetUserAsync(ClaimsPrincipal userPrincipal);
        public Task<bool> UpdateAsync(User user);
    }
}