using System.Globalization;

namespace PlayOn.Common.Helpers
{
    public static class DecimalExtension
    {
        public static string ToFormattedString(this decimal value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

    }
}
