using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.MVC.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class SourcesMapper
    {
        [MapperIgnoreTarget(nameof(Source.Topic))]
        [MapperIgnoreTarget(nameof(Source.Articles))]
        public partial Source SourceSettingsModelToSource(SourceSettingsModel source);

        [MapProperty([nameof(Source.Topic), nameof(Source.Name)], nameof(SourceSettingsPreviewModel.Topic))]
        [MapperIgnoreSource(nameof(Source.SurveyPeriod))]
        [MapperIgnoreSource(nameof(Source.IsRandomPeriod))]
        [MapperIgnoreSource(nameof(Source.HasDynamicPage))]
        [MapperIgnoreSource(nameof(Source.AcceptInsecureCerts))]
        [MapperIgnoreSource(nameof(Source.PageElementLoaded))]
        [MapperIgnoreSource(nameof(Source.PageLoadTimeout))]
        [MapperIgnoreSource(nameof(Source.ElementLoadTimeout))]
        [MapperIgnoreSource(nameof(Source.ArticleCollectionsPath))]
        [MapperIgnoreSource(nameof(Source.ArticleItemPath))]
        [MapperIgnoreSource(nameof(Source.ArticleUrlPath))]
        [MapperIgnoreSource(nameof(Source.ArticleTitlePath))]
        [MapperIgnoreSource(nameof(Source.ArticlePreviewImgPath))]
        [MapperIgnoreSource(nameof(Source.ArticleBodyCollectionsPath))]
        [MapperIgnoreSource(nameof(Source.ArticleBodyItemPath))]
        [MapperIgnoreSource(nameof(Source.ArticlePdatePath))]
        [MapperIgnoreSource(nameof(Source.ArticleTagPath))]
        [MapperIgnoreSource(nameof(Source.TopicId))]
        [MapperIgnoreSource(nameof(Source.Articles))]
        public partial SourceSettingsPreviewModel SourceToSourceSettingPreviewModel(Source source);

        [MapperIgnoreSource(nameof(Source.Topic))]
        [MapperIgnoreSource(nameof(Source.Articles))]
        public partial SourceSettingsModel SourceToSourceSettingsModel(Source? source);
    }
}
