using WebScraper.Core.Loaders.Abstract;

namespace WebScraper.Core.Loaders.Implement
{

    public class StaticPageLoader : IWebLoader
    {
        private readonly HttpClient _httpClient;
        private readonly WebLoaderSettings _webLoaderSettings;  

        public StaticPageLoader(HttpClient httpClient, WebLoaderSettings webLoaderSettings)
        {
            this._httpClient = httpClient;
            this._webLoaderSettings = webLoaderSettings;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoadPageAsync(string url)
        {

            this._httpClient.BaseAddress = new Uri(url);
            var response = await this._httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Failed to load page");
            }
        }
    }
}
