namespace NewsByTheMood.MVC.Models
{
    // Topics and pagination display model
    public class TopicSettingsCollectionModel
    {
        public required TopicSettingsModel[] Topics { get; set; }
        public required PageInfoModel PageInfo { get; set; }
    }
}
