namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of users provider service
    public interface IUserService
    {
        // Is email exists in db
        public Task<bool> IsEmailExistsAsync(string email);

        // Is username exists in db
        public Task<bool> IsUserNameExistsAsync(string username);
    }
}
