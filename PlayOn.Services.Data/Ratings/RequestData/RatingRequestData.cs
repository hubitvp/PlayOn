using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Ratings.RequestData
{
    /// <summary>
    /// Request data for rating the players of a match.
    /// </summary>
    public class RatingRequestData
    {
        /// <summary>
        /// Identifier of the match the players are rated for.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// Identifier of the user giving the ratings.
        /// </summary>
        [Required]
        public Guid RaterId { get; set; }

        /// <summary>
        /// Ratings, one per rated player.
        /// </summary>
        public List<RatingEntryRequestData> Entries { get; set; } = [];
    }

    /// <summary>
    /// The rating of a single player within a <see cref="RatingRequestData"/>.
    /// </summary>
    public class RatingEntryRequestData
    {
        /// <summary>
        /// Identifier of the player being rated.
        /// </summary>
        public Guid RatedUserId { get; set; }

        /// <summary>
        /// Score given to the player.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Optional comment accompanying the score.
        /// </summary>
        public string? Comment { get; set; }
    }
}
