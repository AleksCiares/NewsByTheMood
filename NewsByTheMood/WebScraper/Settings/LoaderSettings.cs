namespace WebScraper.Settings
{
    public class LoaderSettings
    {
        public required bool AcceptInsecureCertificates { get; set; }
        public required string UserAgent { get; set; }
        public ProxySettings? ProxySettings { get; set; }
        public required string SignalElement { get; set; }
        public required TimeSpan PageLoadTimeout { get; set; }
        public required TimeSpan ElementLoadTimeout { get; set; }
    }
}
