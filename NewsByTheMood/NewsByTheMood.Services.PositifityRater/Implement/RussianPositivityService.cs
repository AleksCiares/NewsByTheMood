using System.Globalization;
using System.Text.RegularExpressions;
using Nestor;
using Nestor.Models;
using NewsByTheMood.Services.PositifityRater.Abstract;

namespace NewsByTheMood.Services.PositifityRater.Implement
{
    public static class RussianPositivityService
    {
        private static readonly NestorMorph _nestorMorph = new NestorMorph();
        private static readonly Dictionary<string, WordTonal> _tonalDictionary = ParseTonalDictionaryFromResource();

        /*public RussianPositivityService()
        {
            _nestorMorph = new NestorMorph();
            _tonalDictionary = ParseTonalDictionaryFromResource();
        }*/

        public static short GetPositivity(string text)
        {
            double positivity = 0;
            int wordsCount = 0;
            var rawWords = ExtractWords(text);

            string? lemma = "";
            Word[] wordinfo;
            WordTonal? wordTonal;
            foreach (var rawWord in rawWords)
            {
                wordinfo = _nestorMorph.WordInfo(rawWord);
                if (wordinfo.Length == 0)
                {
                    continue;
                }

                lemma = wordinfo[0].Lemma.Word;
                if (lemma.Length > 0)
                {
                    if (_tonalDictionary.TryGetValue(lemma.ToLower(), out wordTonal))
                    {
                        if (!wordTonal.Tag.Equals("NEUT"))
                        {
                            wordsCount++;
                            positivity += wordTonal.Value;
                        }
                    }
                }
            }

            if (wordsCount > 0)
            {
                positivity /= wordsCount;
            }

            return (short)Math.Round(MapRange(positivity, -1, 1, 1, 10), 0);
        }

        public static List<string> ExtractWords(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return new List<string>();

            // 1. Удаляем HTML-теги
            string text = Regex.Replace(html, "<.*?>", " ");

            // 2. Удаляем знаки препинания (оставляем только буквы и пробелы)
            text = Regex.Replace(text, @"[^\p{L}\s]", " ");

            // 3. Разбиваем на слова и фильтруем однобуквенные
            var words = text
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 1)
                .ToList();

            return words;
        }

        public static Dictionary<string, WordTonal> ParseTonalDictionaryFromResource()
        {
            var assembly = typeof(RussianPositivityService).Assembly;
            var resourceName = "NewsByTheMood.Services.PositivityRater.Resources.tonaldictionary.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);

            var result = new Dictionary<string, WordTonal>();
            string? line;
            bool isFirst = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirst) { isFirst = false; continue; }
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(';');
                if (parts.Length != 8) continue;

                result.Add(
                    parts[0],
                    new WordTonal
                    {
                        Term = parts[0],
                        Tag = parts[1],
                        Value = double.Parse(parts[2], CultureInfo.InvariantCulture),
                        Pstv = double.Parse(parts[3], CultureInfo.InvariantCulture),
                        Ngtv = double.Parse(parts[4], CultureInfo.InvariantCulture),
                        Neut = double.Parse(parts[5], CultureInfo.InvariantCulture),
                        Dunno = double.Parse(parts[6], CultureInfo.InvariantCulture),
                        PstvNgtvDisagreementRatio = double.Parse(parts[7], CultureInfo.InvariantCulture)
                    }
                );
            }

            return result;
        }

        public static double MapRange(double value, double sourceMin, double sourceMax, double targetMin, double targetMax)
        {
            if (value < sourceMin || value > sourceMax)
                throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is out of range [{sourceMin}, {sourceMax}]");

            return targetMin + (value - sourceMin) * (targetMax - targetMin) / (sourceMax - sourceMin);
        }
    }
}
