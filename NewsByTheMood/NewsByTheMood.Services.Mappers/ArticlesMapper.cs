using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.Services.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class ArticlesMapper
    {
        [MapProperty([nameof(Article.Source), nameof(Article.Source.Topic), nameof(Article.Source.Topic.Name)], 
            nameof(ArticlePreviewModel.TopicName))]
        [MapperIgnoreSource(nameof(Article.Url))]
        [MapperIgnoreSource(nameof(Article.Body))]
        [MapperIgnoreSource(nameof(Article.IsActive))]
        [MapperIgnoreSource(nameof(Article.FailedLoaded))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Tags))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticlePreviewModel ArticleToArticlePreviewModel(Article article);

        [MapProperty([nameof(Article.Source), nameof(Article.Source.Topic), nameof(Article.Source.Topic.Name)], 
            nameof(ArticleModel.TopicName))]
        [MapProperty([nameof(Article.Tags)], nameof(ArticleModel.Tags), 
            Use = nameof(TagsListToTagsNameList))]
        [MapperIgnoreSource(nameof(Article.Id))]
        [MapperIgnoreSource(nameof(Article.IsActive))]
        [MapperIgnoreSource(nameof(Article.FailedLoaded))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleModel? ArticleToArticleModel(Article? article);

        [MapProperty([nameof(Article.Source), nameof(Article.Source.Topic), nameof(Article.Source.Topic.Name)],
            nameof(ArticleSettingsPreviewModel.TopicName))]
        [MapperIgnoreSource(nameof(Article.Url))]
        [MapperIgnoreSource(nameof(Article.PreviewImgUrl))]
        [MapperIgnoreSource(nameof(Article.Body))]
        [MapperIgnoreSource(nameof(Article.PublishDate))]
        [MapperIgnoreSource(nameof(Article.Positivity))]
        [MapperIgnoreSource(nameof(Article.Rating))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Tags))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleSettingsPreviewModel ArticleToArticleSettingsPreviewModel(Article article);

        [MapProperty(nameof(ArticleSettingsModel.ArticleTags), nameof(Article.Tags), 
            Use = nameof(TagsNameListToTagsList))]
        [MapperIgnoreTarget(nameof(Article.Source))]
        [MapperIgnoreTarget(nameof(Article.Comments))]
        [MapperIgnoreSource(nameof(ArticleSettingsModel.Sources))]
        [MapperIgnoreSource(nameof(ArticleSettingsModel.Tags))]
        public partial Article ArticleSettingsModelToArticle(ArticleSettingsModel model);

        [MapProperty([nameof(Article.Tags)], nameof(ArticleSettingsModel.ArticleTags), 
            Use = nameof(TagsListToTagsNameList))]
        [MapperIgnoreSource(nameof(Article.Source))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        [MapperIgnoreTarget(nameof(ArticleSettingsModel.Sources))]
        [MapperIgnoreTarget(nameof(ArticleSettingsModel.Tags))]
        public partial ArticleSettingsModel? ArticleToArticleSettingsModel(Article? article);

        [UserMapping]
        private List<Tag> TagsNameListToTagsList(List<string> tags)
        {
            return tags.Select(tag => new Tag()
                    {
                        Name = tag,
                    })
                    .ToList();
        }

        [UserMapping]
        private List<string> TagsListToTagsNameList(List<Tag> tags)
        {
            return tags.Select(tag => tag.Name)
                    .ToList();
        }
    }
}
