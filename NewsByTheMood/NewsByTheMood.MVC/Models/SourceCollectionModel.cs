namespace NewsByTheMood.MVC.Models
{
    // Source preview and pagination display model
    public class SourceCollectionModel
    {
        public required SourcePreviewModel[] SourcePreviews { get; set; }
        public required PageInfoModel PageInfo { get; set; }
    }
}
