namespace WebScraper.Core.Parsers.Abstract
{
    /// <summary>
    /// Interface for parsing a document
    /// </summary>
    public interface IDocumentParser
    {
        internal Task<IDocumentParser> ParseAsync(string document);
        internal void Dispose();
        public IDocumentParser Init(string selector);
        public IDocumentParser Select(string selector);
        public IDocumentParser SelectAll(string selector);
        public string? GetAttribute(string name);
        public List<string> GetAttributes(string name);
        public string TextContent();
        public string ToHtml();
    }
}
