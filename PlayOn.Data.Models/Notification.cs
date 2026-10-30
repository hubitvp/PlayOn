using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.NotificationConstants;


namespace PlayOn.Data.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;


        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

        [MaxLength(LinkMaxLength)]
        public string? Link { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

    }
}
