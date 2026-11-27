namespace PlayOn.Web.Clock
{

    public sealed class ClockOptions
    {
        public ClockMode Mode { get; set; } = ClockMode.System;

        /// <summary>Wall-clock time in the clock's time zone, e.g. "2026-10-01T07:00:00".</summary>
        public DateTime? StartAt { get; set; }

        /// <summary>Optional TimeZoneInfo id. Null = server local time zone.</summary>
        public string? TimeZoneId { get; set; }

    }
}
