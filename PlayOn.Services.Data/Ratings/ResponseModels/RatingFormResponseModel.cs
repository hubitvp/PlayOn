namespace PlayOn.Services.Data.Ratings.ResponseModels
{
    /// <summary>
    /// Response model holding the match and the players shown on the rating form.
    /// </summary>
    public class RatingFormResponseModel
    {
        /// <summary>
        /// Identifier of the match.
        /// </summary>
        public required Guid MatchId { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string MatchTitle { get; init; }
        /// <summary>
        /// Players that can be rated.
        /// </summary>
        public required IReadOnlyList<RatingPlayerResponseModel> Players { get; init; }
    }

    /// <summary>
    /// A player that can be rated on the rating form.
    /// </summary>
    public class RatingPlayerResponseModel
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
