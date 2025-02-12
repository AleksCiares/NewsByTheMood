using System.ComponentModel.DataAnnotations;
using NewsByTheMood.Services.DataProvider.Abstract;

namespace NewsByTheMood.MVC.ValidationAttributes
{
    // Username is exist in db
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IsUserNameExists : ValidationAttribute
    {
        private readonly IUserService _userService;

        public IsUserNameExists(IUserService userService)
        {
            _userService = userService;
        }

        public override bool IsValid(object value)
        {
            var username = value as string;
            if (username is null) return false;
            var result = this._userService.IsUserNameExists(username);
        }
    }
}
