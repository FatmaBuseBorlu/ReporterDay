using System.Text.RegularExpressions;

namespace ReporterDay.PresentationLayer.Helpers
{
    public static class SlugHelper
    {
        public static string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            text = text.ToLowerInvariant()
                       .Replace("ı", "i")
                       .Replace("ğ", "g")
                       .Replace("ü", "u")
                       .Replace("ş", "s")
                       .Replace("ö", "o")
                       .Replace("ç", "c");

            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = Regex.Replace(text, @"\s+", " ").Trim();
            text = text.Replace(" ", "-");
            text = Regex.Replace(text, @"-+", "-");

            return text;
        }
    }
}
