using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a rating one player gives another after a match.
    /// </summary>
    public class PlayerRating
    {
        /// <summary>
        /// Identifier of the match the rating was given for.
        /// </summary>
        [Required]
        [Comment("Identifier of the match the rating was given for")]
        public Guid MatchId { get; set; }

        /// <summary>
        /// The match the rating was given for.
        /// </summary>
        [ForeignKey(nameof(MatchId))]
        public virtual Match Match { get; set; } = null!;

        /// <summary>
        /// Identifier of the user who gave the rating.
        /// </summary>
        [Required]
        [Comment("Identifier of the user who gave the rating")]
        public Guid RaterId { get; set; }

        /// <summary>
        /// The user who gave the rating.
        /// </summary>
        [ForeignKey(nameof(RaterId))]
        public virtual ApplicationUser Rater { get; set; } = null!;

        /// <summary>
        /// Identifier of the user who received the rating.
        /// </summary>
        [Required]
        [Comment("Identifier of the user who received the rating")]
        public Guid RatedUserId { get; set; }

        /// <summary>
        /// The user who received the rating.
        /// </summary>
        [ForeignKey(nameof(RatedUserId))]
        public virtual ApplicationUser RatedUser { get; set; } = null!;

        /// <summary>
        /// Numeric score given to the rated user.
        /// </summary>
        [Required]
        [Comment("Numeric score given to the rated user")]
        public int Score { get; set; }

        /// <summary>
        /// Optional text comment accompanying the rating.
        /// </summary>
        [MaxLength(CommentMaxLength)]
        [Comment("Optional text comment accompanying the rating")]
        public string? Comment { get; set; }

        /// <summary>
        /// Indicates whether the rating is hidden from public view.
        /// </summary>
        [Required]
        [Comment("Indicates whether the rating is hidden from public view")]
        public bool IsHidden { get; set; }
    }
}
