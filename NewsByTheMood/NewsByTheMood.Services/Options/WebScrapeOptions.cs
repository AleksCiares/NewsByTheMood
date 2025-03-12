using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Settings;

namespace NewsByTheMood.Services.Options
{
    public class WebScrapeOptions
    {
        public const string Position = "WebScrape";

        public string[] UserAgents { get; set; } = new string[] 
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
        };
        public required bool UseProxies { get; set; }
        public required bool UseIpRotation { get; set; }
        public ProxySettings[]? Proxies { get; set; }
    }
}
