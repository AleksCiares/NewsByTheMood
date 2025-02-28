namespace NewsByTheMood.MVC.Models
{
    // Topics and pagination display model
    public class TopicCollectionModel
    {
        public required TopicModel[] Topics { get; set; }
        public required PageInfoModel PageInfo { get; set; }
    }
}
