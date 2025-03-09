using AngleSharp;
using AngleSharp.Dom;
using WebScraper.Core.Parsers.Abstract;

namespace WebScraper.Core.Parsers.Implement
{
    public class MyHtmlParser : IDocumentParser<IElement>
    {
        private bool _disposed = false;
        private readonly IBrowsingContext _context;
        private readonly IDocument _document;

        public MyHtmlParser(string html) 
        {
            var config = Configuration.Default.WithDefaultLoader().WithCss();
            this._context = BrowsingContext.New(config);
            this._document = this._context.OpenAsync(req => req.Content(html)).Result;
        }

        public IEnumerable<IElement> SelectAllFromDocument(string selector)
        {
            return this._document.QuerySelectorAll(selector);
        }

        public IElement? SelectFromDocument(string selector)
        {
            return this._document.QuerySelector(selector);
        }

        ~MyHtmlParser()
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
                    this._document.Dispose();
                    this._context.Dispose();
                }
                this._disposed = true;
            }
        }
    }
}
