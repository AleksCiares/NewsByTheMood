using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.MVC.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class ArticleMapper
    {
        [MapProperty([nameof(Article.Source), nameof(Article.Source.Topic), nameof(Article.Source.Topic.Name)], nameof(ArticleDisplayPreviewModel.TopicName))]
        [MapperIgnoreSource(nameof(Article.Url))]
        [MapperIgnoreSource(nameof(Article.Body))]
        [MapperIgnoreSource(nameof(Article.IsActive))]
        [MapperIgnoreSource(nameof(Article.FailedLoaded))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Tags))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleDisplayPreviewModel ArticleToArticleDisplayPreviewModel(Article article);

        [MapProperty([nameof(Article.Source), nameof(Article.Source.Topic), nameof(Article.Source.Topic.Name)], nameof(ArticleDisplayModel.TopicName))]
        [MapProperty([nameof(Article.Tags)], nameof(ArticleDisplayModel.ArticleTags), Use = nameof(TagsListToTagsStringArray))]
        [MapperIgnoreSource(nameof(Article.Id))]
        [MapperIgnoreSource(nameof(Article.IsActive))]
        [MapperIgnoreSource(nameof(Article.FailedLoaded))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleDisplayModel ArticleToArticleDisplayModel(Article article);

        [UserMapping]
        private string[] TagsListToTagsStringArray(List<Tag> tags)
        {
            return tags.Select(t => t.Name).ToArray();
        }
    }
}
