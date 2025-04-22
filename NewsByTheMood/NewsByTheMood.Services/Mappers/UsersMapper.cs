using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.Services.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class UsersMapper
    {
        [MapperIgnoreSource(nameof(User.AccessFailedCount))]
        [MapperIgnoreSource(nameof(User.AvatarUrl))]
        [MapperIgnoreSource(nameof(User.Comments))]
        [MapperIgnoreSource(nameof(User.ConcurrencyStamp))]
        [MapperIgnoreSource(nameof(User.DisplayedName))]
        [MapperIgnoreSource(nameof(User.Email))]
        [MapperIgnoreSource(nameof(User.EmailConfirmed))]
        [MapperIgnoreSource(nameof(User.LockoutEnabled))]
        [MapperIgnoreSource(nameof(User.LockoutEnd))]
        [MapperIgnoreSource(nameof(User.NormalizedEmail))]
        [MapperIgnoreSource(nameof(User.NormalizedUserName))]
        [MapperIgnoreSource(nameof(User.PasswordHash))]
        [MapperIgnoreSource(nameof(User.PhoneNumber))]
        [MapperIgnoreSource(nameof(User.PhoneNumberConfirmed))]
        [MapperIgnoreSource(nameof(User.RegDate))]
        [MapperIgnoreSource(nameof(User.SecurityStamp))]
        [MapperIgnoreSource(nameof(User.TwoFactorEnabled))]
        [MapperIgnoreSource(nameof(User.UserName))]
        [MapProperty(nameof(User.Topics), nameof(UserModel.TopicsIds), Use = nameof(TopicsListToTopicsIdList))]
        public partial UserModel? UserToUserModel(User? user);

        [MapperIgnoreSource(nameof(User.AccessFailedCount))]
        [MapperIgnoreSource(nameof(User.Comments))]
        [MapperIgnoreSource(nameof(User.ConcurrencyStamp))]
        [MapperIgnoreSource(nameof(User.Email))]
        [MapperIgnoreSource(nameof(User.EmailConfirmed))]
        [MapperIgnoreSource(nameof(User.Id))]
        [MapperIgnoreSource(nameof(User.LockoutEnabled))]
        [MapperIgnoreSource(nameof(User.LockoutEnd))]
        [MapperIgnoreSource(nameof(User.NormalizedEmail))]
        [MapperIgnoreSource(nameof(User.NormalizedUserName))]
        [MapperIgnoreSource(nameof(User.PasswordHash))]
        [MapperIgnoreSource(nameof(User.PreferedPositivity))]
        [MapperIgnoreSource(nameof(User.PhoneNumber))]
        [MapperIgnoreSource(nameof(User.PhoneNumberConfirmed))]
        [MapperIgnoreSource(nameof(User.RegDate))]
        [MapperIgnoreSource(nameof(User.SecurityStamp))]
        [MapperIgnoreSource(nameof(User.TwoFactorEnabled))]
        [MapperIgnoreSource(nameof(User.Topics))]
        public partial UserPreviewModel? UserToUserPreviewModel(User? user);

        [UserMapping]
        private List<Int64> TopicsListToTopicsIdList(List<Topic>? topics)
        {
            if (topics == null)
            {
                return new List<Int64>();
            }

            return topics.Select(t => t.Id).ToList();
        }
    }
}
