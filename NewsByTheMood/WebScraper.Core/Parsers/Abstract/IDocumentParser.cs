using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper.Core.Parsers.Abstract
{
    public interface IDocumentParser<T> : IDisposable
    {
        public IEnumerable<T> SelectAllFromDocument(string selector);
        public T? SelectFromDocument(string selector);
    }
}
