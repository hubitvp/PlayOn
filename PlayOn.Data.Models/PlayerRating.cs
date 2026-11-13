using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;


namespace PlayOn.Data.Models
{
    public class PlayerRating
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        public Guid MatchId { get; set; }

        [ForeignKey(nameof(MatchId))]
        public virtual Match Match { get; set; } = null!;


        [Required]
        public Guid RaterId { get; set; }

        [ForeignKey(nameof(RaterId))]
        public virtual ApplicationUser Rater { get; set; } = null!;

        [Required]
        public Guid RatedUserId { get; set; }

        [ForeignKey(nameof(RatedUserId))]
        public virtual ApplicationUser RatedUser { get; set; } = null!;

        [Required]
        public int Score { get; set; }

        [MaxLength(CommentMaxLength)]
        public string? Comment { get; set; }

        [Required]
        public bool IsHidden { get; set; }


    }
}
