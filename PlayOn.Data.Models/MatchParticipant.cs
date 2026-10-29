using PlayOn.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    public class MatchParticipant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [ForeignKey(nameof(MatchId))]
        public virtual Match Match { get; set; } = null!;


        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;



        [Required]
        public ParticipantStatus Status { get; set; }


        [Required]
        public DateTime JoinedOn { get; set; }

        [Required]
        public DateTime StatusChangedOn { get; set; }

        [Required]
        public bool IsLateCancel { get; set; }

    }
}
