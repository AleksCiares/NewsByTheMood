using NewsByTheMood.Data.Entities;
using NewsByTheMood.MVC.Models;
using Riok.Mapperly.Abstractions;

namespace NewsByTheMood.MVC.Mappers
{
    [Mapper(AutoUserMappings = false)]
    public partial class TopicsMapper
    {
        [MapperIgnoreTarget(nameof(Topic.Sources))]
        [MapperIgnoreTarget(nameof(Topic.Users))]
        public partial Topic TopicSettingsModelToTopic(TopicSettingsModel topic);

        [MapperIgnoreSource(nameof(Topic.Sources))]
        [MapperIgnoreSource(nameof(Topic.Users))]
        public partial TopicSettingsModel TopicToTopicSettingsModel(Topic topic);
    }
}
