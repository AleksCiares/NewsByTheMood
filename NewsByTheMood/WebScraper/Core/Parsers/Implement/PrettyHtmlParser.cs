using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using System.Text;
using WebScraper.Core.Parsers.Abstract;

namespace WebScraper.Core.Parsers.Implement
{
    internal sealed class PrettyHtmlParser : IDocumentParser
    {
        private bool _disposed = false;
        private readonly IBrowsingContext _context;
        private IDocument? _document;
        private List<IElement>? _elements = null;

        internal PrettyHtmlParser() 
        {
            var config = Configuration.Default.WithDefaultLoader().WithCss();
            _context = BrowsingContext.New(config);
        }

        async Task<IDocumentParser> IDocumentParser.ParseAsync(string document)
        {
            _document = await _context.OpenAsync(req => req.Content(document));
            if (_elements != null)
            {
                _elements.Clear();
            }
            _elements = null;
            return this;
        }

        public IDocumentParser Init(string selector)
        {
            if (_document == null)
            {
                throw new Exception();
            }
            if (_elements != null)
            {
                _elements.Clear();
            }
            _elements = null;
            _elements = _document.QuerySelectorAll(selector).ToList();

            return this;
        }

        public IDocumentParser Select(string selector)
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            IElement? el = null;
            foreach (var element in _elements)
            {
                el = element.QuerySelector(selector);
                if (el != null)
                {
                    _elements.Clear();
                    _elements.Add(el);
                    break;
                }
            }

            return this;
        }

        public IDocumentParser SelectAll(string selector)
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            var newElements = new List<IElement>();
            foreach (var element in _elements)
            {
                newElements.AddRange(element.QuerySelectorAll(selector).ToArray());
            }

            _elements.Clear();
            _elements = null;
            _elements = newElements;

            return this;
        }

        public string? GetAttribute(string name)
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            string? attribute = null;
            foreach (var element in _elements)
            {
                attribute = element.GetAttribute(name);
                if (attribute != null)
                {
                    break;
                }
            }

            return attribute;
        }

        public List<string> GetAttributes(string name)
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            List<string> attributes = new List<string>();
            foreach (var element in _elements)
            {
                var attribute = element.GetAttribute(name);
                if (attribute != null)
                {
                    attributes.Add(attribute);
                }
            }

            return attributes;
        }

        public string TextContent()
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            var text = new StringBuilder();
            foreach (var element in _elements)
            {
                text.Append(element.TextContent);
            }
            
            return text.ToString();
        }

        public string ToHtml()
        {
            if (_elements == null)
            {
                throw new Exception();
            }

            var html = new StringBuilder();
            var htmlMarkupFormatter = new PrettyMarkupFormatter();
            foreach (var element in _elements)
            {
                html.Append(element.ToHtml(htmlMarkupFormatter));
            }

            return html.ToString();
        }

        void IDocumentParser.Dispose()
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
                    if (_elements != null)
                    {
                        _elements.Clear();
                    }
                    if(_document != null)
                    {
                        _document.Dispose();
                    }
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        ~PrettyHtmlParser()
        {
            Dispose(false);
        }
    }
}
