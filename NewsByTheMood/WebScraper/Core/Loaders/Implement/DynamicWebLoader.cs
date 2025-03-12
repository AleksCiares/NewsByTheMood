using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebScraper.Core.Loaders.Abstract;
using WebScraper.Settings;

namespace WebScraper.Core.Loaders.Implement
{
    internal sealed class DynamicWebLoader : IWebLoader
    {
        private bool _disposed = false;
        private readonly IWebDriver _webDriver;
        private readonly LoaderSettings _settings;
        private readonly string _jsScrollScript = @"window.scrollBy(0, 50)";

        internal DynamicWebLoader(LoaderSettings settings)
        {
            _settings = settings;
            NetworkAuthenticationHandler? proxyAuthHandler = null;

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument($"--user-agent={_settings.UserAgent}");
            options.AcceptInsecureCertificates = _settings.AcceptInsecureCertificates;
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            if (_settings.ProxySettings != null)
            {
                options.AddArgument($"--proxy-server=" +
                    $"{_settings.ProxySettings.Host}:" +
                    $"{_settings.ProxySettings.Port}");
                
                if (_settings.ProxySettings.Username != null)
                {
                    proxyAuthHandler = new NetworkAuthenticationHandler()
                    {
                        Credentials = new PasswordCredentials(
                           _settings.ProxySettings.Username,
                           _settings.ProxySettings.Password)
                    };
                }
            }

            _webDriver = new ChromeDriver(options);
            if (proxyAuthHandler != null)
            {
                (_webDriver.Manage().Network).AddAuthenticationHandler(proxyAuthHandler);
            }
            _webDriver.Manage().Timeouts().PageLoad = _settings.PageLoadTimeout;
        }

        public Task DownloadFile(string url, string storePath)
        {
            throw new NotImplementedException();
        }

        async Task<string> IWebLoader.LoadPageAsync(string url)
        {
            await _webDriver.Navigate().GoToUrlAsync(url);
            ((IJavaScriptExecutor)this._webDriver).ExecuteScript(this._jsScrollScript);
            var wait = new WebDriverWait(this._webDriver, this._settings.ElementLoadTimeout);
            var controlledElement = wait.Until(
                c => c.FindElement(By.CssSelector(this._settings.SignalElement)));

            return _webDriver.PageSource;
        }

        void IWebLoader.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _webDriver.Quit();
                    _webDriver.Dispose();
                }
                _disposed = true;
            }
        }

        ~DynamicWebLoader()
        {
            Dispose(false);
        }
    }
}