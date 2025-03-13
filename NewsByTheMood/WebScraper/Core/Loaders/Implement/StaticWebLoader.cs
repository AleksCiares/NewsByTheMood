using WebScraper.Core.Loaders.Abstract;
using WebScraper.Settings;

namespace WebScraper.Core.Loaders.Implement
{
    internal sealed class StaticWebLoader : IWebLoader
    {
        private bool _disposed = false;
        private readonly HttpClient _httpClient;
        private readonly LoaderSettings _settings;

        internal StaticWebLoader(LoaderSettings settings)
        {
            _httpClient = new HttpClient();
            _settings = settings;
        }

        async Task<string> IWebLoader.LoadAsync(string url)
        {
            _httpClient.BaseAddress = new Uri(url);
            var response = await _httpClient.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        public Task DownloadFile(string url, string storePath)
        {
            throw new NotImplementedException();
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
                    _httpClient.Dispose();
                }
                _disposed = true;
            }
        }

        ~StaticWebLoader()
        {
            Dispose(false);
        }
    }
}
