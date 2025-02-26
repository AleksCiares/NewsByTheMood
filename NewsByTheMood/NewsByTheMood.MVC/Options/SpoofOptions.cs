namespace NewsByTheMood.MVC.Options
{
    public class SpoofOptions
    {
        public const string Position = "Spoofing";
        public bool SpoofRealId { get; set; } = false;
        public string SpoofSecret { get; set; } = "";
    }
}
