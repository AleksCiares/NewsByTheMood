namespace WebScraper.Core.Parsers.Abstract
{
    /// <summary>
    /// Interface for parsing a document
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocumentParser<T> : IDisposable
    {
        public IEnumerable<T> SelectAllFromDocument(string selector);
        public T? SelectFromDocument(string selector);
    }
}
