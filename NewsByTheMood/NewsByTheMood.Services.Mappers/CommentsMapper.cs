using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.Services.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class CommentsMapper
    {
        [MapProperty([nameof(Comment.User), nameof(Comment.User.AvatarUrl)], nameof(CommentModel.UserAvatar))]
        [MapProperty([nameof(Comment.User), nameof(Comment.User.DisplayedName)], nameof(CommentModel.UserDisplayName))]
        [MapProperty([nameof(Comment.User), nameof(Comment.User.UserName)], nameof(CommentModel.UserName))]
        [MapperIgnoreSource(nameof(Comment.Id))]
        [MapperIgnoreSource(nameof(Comment.Position))]
        [MapperIgnoreSource(nameof(Comment.ArticleId))]
        [MapperIgnoreSource(nameof(Comment.UserId))]
        [MapperIgnoreSource(nameof(Comment.Article))]
        public partial CommentModel CommentToCommentModel(Comment comment);

        [MapProperty([nameof(Comment.User), nameof(Comment.User.AvatarUrl)], nameof(CommentModel.UserAvatar))]
        [MapProperty([nameof(Comment.User), nameof(Comment.User.DisplayedName)], nameof(CommentModel.UserDisplayName))]
        [MapProperty([nameof(Comment.User), nameof(Comment.User.UserName)], nameof(CommentModel.UserName))]
        [MapperIgnoreSource(nameof(Comment.Position))]
        [MapperIgnoreSource(nameof(Comment.ArticleId))]
        [MapperIgnoreSource(nameof(Comment.UserId))]
        [MapperIgnoreSource(nameof(Comment.Article))]
        public partial CommentSettingsModel CommentToCommentSettingsModel(Comment comment);
    }
}
