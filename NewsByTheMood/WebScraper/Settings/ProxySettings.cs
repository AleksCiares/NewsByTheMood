namespace WebScraper.Settings
{
    /// <summary>
    /// Web loader proxy settings
    /// </summary>
    public class ProxySettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
