namespace WebScraper.Core.Settings
{
    /// <summary>
    /// Settings for the web loader
    /// </summary>
    public class WebLoaderSettings
    {
        public required string UserAgent { get; set; }
        public ProxySettings? ProxySettings { get; set; }
        public required bool AcceptInsecureCertificates { get; set; }
        /// <summary> 
        /// Css selector for the element to wait for full page load
        /// </summary>
        public required string PageElementLoaded { get; set; }
        public required TimeSpan PageLoadTimeout { get; set; }
        public required TimeSpan ElementLoadTimeout { get; set; }
    }
}
