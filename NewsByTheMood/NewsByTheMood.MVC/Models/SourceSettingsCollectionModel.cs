namespace NewsByTheMood.MVC.Models
{
    // Source preview and pagination display model
    public class SourceSettingsCollectionModel
    {
        public required SourceSettingsPreviewModel[] SourcePreviews { get; set; }
        public required PageInfoModel PageInfo { get; set; }
    }
}
