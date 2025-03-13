namespace WebScraper.Core.Loaders.Abstract
{
    /// <summary>
    /// Interface for loading a web page
    /// </summary>
    public interface IWebLoader
    {
        internal Task<string> LoadAsync(string url);
        internal void Dispose();
        public Task DownloadFile(string url, string storePath);
    }
}
