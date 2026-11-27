namespace PlayOn.Services.Data.Match.ResponseModels
{
    /// <summary>
    /// Response model with the details of a cancelled match, used to confirm the cancellation and notify participants.
    /// </summary>
    public class MatchCancelResponseModel
    {
        /// <summary>
        /// Title of the cancelled match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Time the cancelled match was due to start.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Date of the cancelled match.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Name of the venue where the match was to be played.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// Number of players who had joined the match.
        /// </summary>
        public required int JoinedCount { get; init; }
    }
}
