using System.Globalization;

namespace PlayOn.Web.DataBuilders.Formatting
{
    public sealed class DisplayFormatter : IDisplayFormatter
    {
        /// <summary>Wednesday, 11 November 2026, 18:30</summary>
        public const string DateTimeFormat = "dddd, dd MMMM yyyy, HH:mm";

        /// <summary>Wednesday, 11 November  18:30</summary>
        public const string ShortDateTimeFormat = "dddd, dd MMMM  HH:mm";

        /// <summary>Wednesday, 11 November 2026</summary>
        public const string DateFormat = "dddd, dd MMMM yyyy";

        /// <summary>Wed 11 Nov 2026</summary>
        public const string ShortDateFormat = "ddd dd MMM yyyy";

        /// <summary>18:30</summary>
        public const string TimeFormat = "HH:mm";

        /// <summary>7.50</summary>
        public const string PriceFormat = "0.00";


        public static readonly CultureInfo Culture = CultureInfo.InvariantCulture;


        public string DateTime(DateTime value) => value.ToString(DateTimeFormat, Culture);

        public string ShortDateTime(DateTime value) => value.ToString(ShortDateTimeFormat, Culture);

        public string Date(DateOnly value) => value.ToString(DateFormat, Culture);

        public string ShortDate(DateOnly value) => value.ToString(ShortDateFormat, Culture);

        public string Time(TimeOnly value) => value.ToString(TimeFormat, Culture);

        public string Time(DateTime value) => value.ToString(TimeFormat, Culture);

        public string Price(decimal value) => value.ToString(PriceFormat, Culture);
    }
}
