using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;

namespace PlayOn.Web.ViewModels.Ratings
{
    /// <summary>
    /// Model of the form for rating the players of a match.
    /// </summary>
    public class RatingFormViewModel
    {
        /// <summary>
        /// Identifier of the match.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The match is not valid.")]
        public Guid MatchId { get; set; }

        /// <summary>
        /// Title of the match.
        /// </summary>
        [ValidateNever]
        public string MatchTitle { get; set; } = null!;

        /// <summary>
        /// Players that can be rated.
        /// </summary>
        public List<RatingPlayerInputModel> Players { get; set; } = [];
    }

    /// <summary>
    /// The rating of a single player on the rating form.
    /// </summary>
    public class RatingPlayerInputModel
    {
        /// <summary>
        /// Identifier of the player being rated.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The player is not valid.")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Display name of the player.
        /// </summary>
        [ValidateNever]
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// City of the player.
        /// </summary>
        [ValidateNever]
        public string City { get; set; } = null!;

        /// <summary>
        /// Score given to the player; may be left empty to skip the player.
        /// </summary>
        [Range(MinRatingScore, MaxRatingScore, ErrorMessage = "The score must be between {1} and {2}.")]
        public int? Score { get; set; }

        /// <summary>
        /// Optional comment accompanying the score.
        /// </summary>
        [StringLength(CommentMaxLength)]
        public string? Comment { get; set; }
    }
}