using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.Services.MVC.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class TopicsMapper
    {
        [MapperIgnoreSource(nameof(TopicSettingsModel.RelatedSources))]
        [MapperIgnoreTarget(nameof(Topic.Sources))]
        [MapperIgnoreTarget(nameof(Topic.Users))]
        public partial Topic TopicSettingsModelToTopic(TopicSettingsModel topic);

        [MapProperty([nameof(Topic.Sources), nameof(Topic.Sources.Count)], nameof(TopicSettingsModel.RelatedSources))]
        [MapperIgnoreSource(nameof(Topic.Users))]
        public partial TopicSettingsModel? TopicToTopicSettingsModel(Topic? topic);

        [MapperIgnoreSource(nameof(Topic.Id))]
        [MapperIgnoreSource(nameof(Topic.Users))]
        [MapperIgnoreSource(nameof(Topic.Sources))]
        public partial TopicModel TopicToTopicModel(Topic topic);

        [MapperIgnoreSource(nameof(Topic.IconCssClass))]
        [MapperIgnoreSource(nameof(Topic.Users))]
        [MapperIgnoreSource(nameof(Topic.Sources))]
        public partial TopicSearchModel? TopicToTopicSearchModel(Topic? topic);
    }
}
