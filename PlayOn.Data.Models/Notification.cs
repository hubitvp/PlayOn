using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.NotificationConstants;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a notification sent to a user, optionally related to a match.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Unique identifier of the notification.
        /// </summary>
        [Key]
        [Comment("Unique identifier of the notification")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Identifier of the user receiving the notification.
        /// </summary>
        [Required]
        [Comment("Identifier of the user receiving the notification")]
        public Guid UserId { get; set; }

        /// <summary>
        /// The user receiving the notification.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Type of the notification.
        /// </summary>
        [Required]
        [Comment("Type of the notification")]
        public NotificationType Type { get; set; }

        /// <summary>
        /// Text content of the notification.
        /// </summary>
        [Required]
        [MaxLength(MessageMaxLength)]
        [Comment("Text content of the notification")]
        public string Message { get; set; } = null!;

        /// <summary>
        /// Optional identifier of the match the notification relates to.
        /// </summary>
        [Comment("Optional identifier of the match the notification relates to")]
        public Guid? MatchId { get; set; }

        /// <summary>
        /// The match the notification relates to, if any.
        /// </summary>
        [ForeignKey(nameof(MatchId))]
        public virtual Match? Match { get; set; }

        /// <summary>
        /// Indicates whether the user has read the notification.
        /// </summary>
        [Required]
        [Comment("Indicates whether the user has read the notification")]
        public bool IsRead { get; set; }

        /// <summary>
        /// Date and time the notification was created.
        /// </summary>
        [Required]
        [Comment("Date and time the notification was created")]
        public DateTime CreatedOn { get; set; }
    }
}
