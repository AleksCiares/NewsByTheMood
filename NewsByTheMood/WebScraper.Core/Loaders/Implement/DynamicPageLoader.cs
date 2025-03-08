using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Settings;

namespace WebScraper.Core.Loaders.Implement
{
    public class DynamicPageLoader : IWebLoader
    {
        private bool _disposed = false;
        private readonly IWebDriver _webDriver;
        private readonly WebLoaderSettings _settings;
        private readonly string _jsScrollScript = @"window.scrollBy(0, 50)";

        public DynamicPageLoader(WebLoaderSettings settings)
        {
            this._settings = settings;
            NetworkAuthenticationHandler? proxyAuthHandler = null;

            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument($"--user-agent={this._settings.UserAgent}");
            options.AcceptInsecureCertificates = this._settings.AcceptInsecureCertificates;
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            if (this._settings.ProxySettings != null)
            {
                options.AddArgument($"--proxy-server=" +
                    $"{this._settings.ProxySettings.Host}:" +
                    $"{this._settings.ProxySettings.Port}");
                
                if (this._settings.ProxySettings.Username != null)
                {
                    proxyAuthHandler = new NetworkAuthenticationHandler()
                    {
                        Credentials = new PasswordCredentials(
                           this._settings.ProxySettings.Username,
                           this._settings.ProxySettings.Password)
                    };
                }
            }


            this._webDriver = new ChromeDriver(options);
            if (proxyAuthHandler != null)
            {
                (this._webDriver.Manage().Network).AddAuthenticationHandler(proxyAuthHandler);
            }
            this._webDriver.Manage().Timeouts().PageLoad = this._settings.PageLoadTimeout;
        }

        ~DynamicPageLoader()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._webDriver.Quit();
                    this._webDriver.Dispose();
                }
                this._disposed = true;
            }
        }

        public async Task<string> LoadPageAsync(string url)
        {
            await this._webDriver.Navigate().GoToUrlAsync(url);
            ((IJavaScriptExecutor)this._webDriver).ExecuteScript(this._jsScrollScript);
            var wait = new WebDriverWait(this._webDriver, this._settings.ElementLoadTimeout);
            var controlledElement = wait.Until(
                c => c.FindElement(By.CssSelector(this._settings.PageElementLoaded)));
                  
            return this._webDriver.PageSource;
        }
    }
}