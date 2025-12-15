using System.Text.RegularExpressions;

namespace HungryOClockV2.Models.Helpers
{
    public class SlugHelper
    {
        public static string GenerateSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            value = value.ToLowerInvariant();
            value = Regex.Replace(value, @"[^a-z0-9\s-]", "");
            value = Regex.Replace(value, @"\s+", "-").Trim('-');
            value = Regex.Replace(value, "-{2,}", "-");
            return value;
        }
    }
}
