namespace WebScraper.Core.Loaders.Abstract
{
    /// <summary>
    /// Interface for loading a web page
    /// </summary>
    public interface IWebLoader : IDisposable
    {
        public Task<string> LoadPageAsync(string url);
    }
}
