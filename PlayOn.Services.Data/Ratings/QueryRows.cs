using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Ratings
{
    /// <summary>
    /// Query row (projection) with the match information needed to rate its players.
    /// </summary>
    public sealed class RatingInfoRow
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public MatchStatus Status { get; set; }
        /// <summary>
        /// Soft-delete flag indicating whether the match has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
