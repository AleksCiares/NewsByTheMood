using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using NewsByTheMood.MVC.Options;
using NewsByTheMood.Services.FileProvider.Abstract;

namespace NewsByTheMood.Services.FileProvider.Implement
{
    // Service for provide user icons for elements
    public class LocalIconService : IiconService
    {
        private readonly UserIconsOptions _options;

        public LocalIconService(IOptions<UserIconsOptions> options)
        {
            this._options = options.Value;
        }

        // Get all icon css classes
        public async Task<string[]> GetIconsCssClasses()
        {
            if (string.IsNullOrEmpty(this._options.CssFilePath) || 
                string.IsNullOrEmpty(this._options.CssClassRegex) || 
                !File.Exists(this._options.CssFilePath))
            {
                throw new ArgumentException("LocalIconsService. Invalid icons service options");
            }

            FileStream? fstream = null;
            string? textFromFile = string.Empty;
            var iconsCss = new List<string>();

            try
            {
                fstream = File.OpenRead(this._options.CssFilePath);
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                textFromFile = Encoding.Default.GetString(buffer);

                var regex = new Regex(this._options.CssClassRegex, RegexOptions.Compiled);
                var matches = regex.Matches(textFromFile);

                if (matches.Count > 0)
                {
                    if (this._options.BaseCssClasses != null && this._options.BaseCssClasses.Length > 0)
                    {
                        var iconBaseCsses = string.Empty;
                        foreach (var baseCss in this._options.BaseCssClasses)
                        {
                            iconBaseCsses += baseCss + " ";
                        }
                        foreach (Match match in matches)
                        {
                            iconsCss.Add(iconBaseCsses + match.Value);
                        }
                    }
                    else
                    {
                        foreach (Match match in matches)
                        {
                            iconsCss.Add(match.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iconsCss?.Clear();
                throw new Exception(ex.Message);
            }
            finally
            {
                fstream?.Close();
                textFromFile = null;
            }

            return iconsCss.ToArray();
        }
    }
}
