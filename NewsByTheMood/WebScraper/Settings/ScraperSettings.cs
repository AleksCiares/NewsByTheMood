namespace WebScraper.Settings
{
    public class ScraperSettings
    {
        public required bool IsDynamicSource { get; set; }
        public required LoaderSettings LoaderSettings { get; set; }
    }
}
