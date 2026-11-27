using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Attendance
{
    /// <summary>
    /// Query row (projection) with the match information needed to take attendance.
    /// </summary>
    public sealed class AttendanceInfoRow
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Soft-delete flag indicating whether the match has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Identifier of the user who organises the match.
        /// </summary>
        public Guid OrganiserId { get; set; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public MatchStatus Status { get; set; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string VenueName { get; set; } = null!;
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Date on which the match takes place.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }
    }
}
