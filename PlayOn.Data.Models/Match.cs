using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.MatchConstants;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a match organised by a member, which other members can join.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        [Key]
        [Comment("Unique identifier of the match")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identifier of the user who organises the match.
        /// </summary>
        [Required]
        [Comment("Identifier of the user who organises the match")]
        public Guid OrganiserId { get; set; }

        /// <summary>
        /// The user who organises the match.
        /// </summary>
        [ForeignKey(nameof(OrganiserId))]
        public virtual ApplicationUser Organiser { get; set; } = null!;

        /// <summary>
        /// Title of the match.
        /// </summary>
        [Required]
        [MaxLength(TitleMaxLength)]
        [Comment("Title of the match")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Optional description of the match.
        /// </summary>
        [MaxLength(DescriptionMaxLength)]
        [Comment("Optional description of the match")]
        public string? Description { get; set; }

        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        [Required]
        [Comment("Duration of the match in minutes")]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        [Required]
        [Comment("Minimum number of players required for the match to take place")]
        public int MinPlayers { get; set; }

        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        [Required]
        [Comment("Maximum number of players allowed to join the match")]
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        [Required]
        [Comment("Minimum skill level required to join the match")]
        public SkillLevel MinSkill { get; set; }

        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        [Required]
        [Comment("Maximum skill level allowed to join the match")]
        public SkillLevel MaxSkill { get; set; }

        /// <summary>
        /// Price each player pays to participate in the match.
        /// </summary>
        [Required]
        [Comment("Price each player pays to participate in the match")]
        public decimal PricePerPlayer { get; set; }

        /// <summary>
        /// Current status of the match.
        /// </summary>
        [Required]
        [Comment("Current status of the match")]
        public MatchStatus Status { get; set; }

        /// <summary>
        /// Optional reason given when the match was cancelled.
        /// </summary>
        [MaxLength(CancelReasonMaxLength)]
        [Comment("Optional reason given when the match was cancelled")]
        public string? CancelReason { get; set; }

        /// <summary>
        /// Soft-delete flag indicating whether the match has been deleted.
        /// </summary>
        [Required]
        [Comment("Soft-delete flag indicating whether the match has been deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Concurrency token used for optimistic concurrency control.
        /// </summary>
        [Required]
        [Comment("Concurrency token used for optimistic concurrency control")]
        public byte[] RowVersion { get; set; } = null!;

        /// <summary>
        /// Collection of participants who joined the match.
        /// </summary>
        public virtual ICollection<MatchParticipant> Participants { get; set; }
            = new List<MatchParticipant>();

        /// <summary>
        /// Collection of player ratings given after the match.
        /// </summary>
        public virtual ICollection<PlayerRating> Ratings { get; set; }
            = new List<PlayerRating>();

        /// <summary>
        /// The scheduled event (sport, court, venue) of the match.
        /// </summary>
        public virtual Event? Event { get; set; } = null!;
    }
}
