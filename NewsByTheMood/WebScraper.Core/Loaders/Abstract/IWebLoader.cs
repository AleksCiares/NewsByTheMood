namespace WebScraper.Core.Loaders.Abstract
{
    public interface IWebLoader : IDisposable
    {
        public Task<string> LoadPageAsync(string url);
    }
}
