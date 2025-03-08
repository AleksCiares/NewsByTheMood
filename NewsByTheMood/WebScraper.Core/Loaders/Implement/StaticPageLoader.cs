using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Settings;

namespace WebScraper.Core.Loaders.Implement
{

    public class StaticPageLoader : IWebLoader
    {
        private bool _disposed = false;
        private readonly HttpClient _httpClient;
        private readonly WebLoaderSettings _settings;  

        public StaticPageLoader(WebLoaderSettings webLoaderSettings)
        {
            this._httpClient = new HttpClient();
            this._settings = webLoaderSettings;
        }

        ~StaticPageLoader()
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
                    this._httpClient.Dispose();
                }
                this._disposed = true;
            }
        }

        public async Task<string> LoadPageAsync(string url)
        {

            this._httpClient.BaseAddress = new Uri(url);
            var response = await this._httpClient.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
