using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a user's participation in a match (many-to-many between users and matches).
    /// </summary>
    public class MatchParticipant
    {
        /// <summary>
        /// Identifier of the match.
        /// </summary>
        [Required]
        [Comment("Identifier of the match")]
        public Guid MatchId { get; set; }

        /// <summary>
        /// The match the user participates in.
        /// </summary>
        [ForeignKey(nameof(MatchId))]
        public virtual Match Match { get; set; } = null!;

        /// <summary>
        /// Identifier of the participating user.
        /// </summary>
        [Required]
        [Comment("Identifier of the participating user")]
        public Guid UserId { get; set; }

        /// <summary>
        /// The participating user.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Current status of the participation (e.g. joined, waitlisted, cancelled).
        /// </summary>
        [Required]
        [Comment("Current status of the participation")]
        public ParticipantStatus Status { get; set; }

        /// <summary>
        /// Date and time the user joined the match.
        /// </summary>
        [Required]
        [Comment("Date and time the user joined the match")]
        public DateTime JoinedOn { get; set; }

        /// <summary>
        /// Date and time the participation status last changed.
        /// </summary>
        [Required]
        [Comment("Date and time the participation status last changed")]
        public DateTime StatusChangedOn { get; set; }

        /// <summary>
        /// Indicates whether the user cancelled too close to the start of the match.
        /// </summary>
        [Required]
        [Comment("Indicates whether the user cancelled too close to the start of the match")]
        public bool IsLateCancel { get; set; }
    }
}
