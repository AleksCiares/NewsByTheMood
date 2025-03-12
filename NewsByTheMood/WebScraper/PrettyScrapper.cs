﻿using WebScraper.Core.Loaders.Abstract;
using WebScraper.Core.Loaders.Implement;
using WebScraper.Core.Parsers.Abstract;
using WebScraper.Core.Parsers.Implement;
using WebScraper.Settings;

namespace WebScraper
{
    public class PrettyScrapper : IDisposable
    {
        private bool _disposed = false;
        private readonly ScraperSettings _settings;
        private readonly IWebLoader _loader;
        private readonly IDocumentParser _parser;

        public PrettyScrapper(ScraperSettings settings)
        {
            _settings = settings;
            _loader = settings.IsDynamicSource
                ? new DynamicWebLoader(_settings.LoaderSettings)
                : new StaticWebLoader(_settings.LoaderSettings);
            _parser = new PrettyHtmlParser();
        }

        public async Task LoadPage(string url)
        {
            await _parser.ParseAsync(await _loader.LoadPageAsync(url));
        }

        public IWebLoader Loader
        {
            get { return _loader; }
        }

        public IDocumentParser Parser 
        { 
            get { return _parser; } 
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _loader.Dispose();
                    _parser.Dispose();
                }
                _disposed = true;
            }
        }

        ~PrettyScrapper()
        {
            Dispose(false);
        }

    }
}
