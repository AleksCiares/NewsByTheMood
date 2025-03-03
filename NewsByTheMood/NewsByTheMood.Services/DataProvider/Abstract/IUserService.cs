namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of users provider service
    public interface IUserService
    {
        public Task<bool> IsEmailExistsAsync(string email);
        public Task<bool> IsUserNameExistsAsync(string username);
    }
}
