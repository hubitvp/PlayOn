using PlayOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
