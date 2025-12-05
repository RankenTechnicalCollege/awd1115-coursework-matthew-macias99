using System.Text;
using System.Text.RegularExpressions;

namespace HOT3.Models
{
    public class SlugHelper
    {
        public static string Slugify(string text)
        {
            text = text.ToLowerInvariant().Trim();
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
            text = Regex.Replace(text, @"\s+", " ").Trim();
            text = text.Replace(" ", "-");
            return text;
        }
    }
}
