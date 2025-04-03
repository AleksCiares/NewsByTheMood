using Microsoft.AspNetCore.Mvc.Rendering;
using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.MVC.Mappers
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
            Use = nameof(TagsListToTagsNameArray))]
        [MapperIgnoreSource(nameof(Article.Id))]
        [MapperIgnoreSource(nameof(Article.IsActive))]
        [MapperIgnoreSource(nameof(Article.FailedLoaded))]
        [MapperIgnoreSource(nameof(Article.SourceId))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleModel ArticleToArticleModel(Article article);

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

        [MapProperty(nameof(ArticleSettingsModel.Tags), nameof(Article.Tags), 
            Use = nameof(TagsSelectListToTagsList))]
        [MapperIgnoreTarget(nameof(Article.Source))]
        [MapperIgnoreTarget(nameof(Article.Comments))]
        public partial Article ArticleSettingsModelToArticle(ArticleSettingsModel model);

        [MapProperty([nameof(Article.Tags)], nameof(ArticleSettingsModel.Tags), 
            Use = nameof(TagsListToTagsSelectList))]
        [MapperIgnoreSource(nameof(Article.Source))]
        [MapperIgnoreSource(nameof(Article.Comments))]
        public partial ArticleSettingsModel? ArticleToArticleSettingsModel(Article? article);

        [UserMapping]
        private string[] TagsListToTagsNameArray(List<Tag> tags)
        {
            return tags.Select(t => t.Name).ToArray();
        }

        [UserMapping]
        private List<Tag> TagsSelectListToTagsList(List<SelectListItem> tags)
        {
            return tags.Where(tag => tag.Selected)
                    .Select(tag => new Tag()
                    {
                        Id = Int64.Parse(tag.Value),
                        Name = tag.Text
                    })
                    .ToList();
        }

        [UserMapping]
        private List<SelectListItem> TagsListToTagsSelectList(List<Tag> tags)
        {
            return tags.Select(tag => new SelectListItem()
                    {
                        Value = tag.Id.ToString(),
                        Text = tag.Name,
                        Selected = true
                    })
                    .ToList();
        }
    }
}
