using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PlayOn.Common.Helpers
{
    public static class Slug
    {
        public static string Create(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
                sb.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-');
            }
            return Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        }
    }
}
