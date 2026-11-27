namespace PlayOn.Services.Data.Attendance.ResponseModels
{
    /// <summary>
    /// Response model holding the match details and players shown on the attendance form.
    /// </summary>
    public class AttendanceResponseModel
    {
        /// <summary>
        /// Identifier of the match.
        /// </summary>
        public required Guid MatchId { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Date on which the match takes place.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }
        /// <summary>
        /// Players of the match.
        /// </summary>
        public required IReadOnlyList<AttendancePlayerResponseModel> Players { get; init; }
    }

    /// <summary>
    /// A player listed on the attendance form.
    /// </summary>
    public class AttendancePlayerResponseModel
    {
        /// <summary>
        /// Identifier of the player.
        /// </summary>
        public required Guid UserId { get; init; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// City of the player.
        /// </summary>
        public required string City { get; init; }
    }
}
