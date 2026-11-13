using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PlayOn.Common.EntityConstants.MatchConstants;


namespace PlayOn.Data.Models
{
    public class Match
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrganiserId { get; set; }


        [ForeignKey(nameof(OrganiserId))]
        public virtual ApplicationUser Organiser { get; set; } = null!;



        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }


        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public int MinPlayers { get; set; }

        [Required]
        public int MaxPlayers { get; set; }


        [Required]
        public SkillLevel MinSkill { get; set; }

        [Required]
        public SkillLevel MaxSkill { get; set; }


        [Required]
        public decimal PricePerPlayer { get; set; }


        [Required]
        public MatchStatus Status { get; set; }

        [MaxLength(CancelReasonMaxLength)]
        public string? CancelReason { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        [Required]
        public byte[] RowVersion { get; set; } = null!;



        public virtual ICollection<MatchParticipant> Participants { get; set; }
            = new List<MatchParticipant>();

        public virtual ICollection<PlayerRating> Ratings { get; set; }
            = new List<PlayerRating>();


        public virtual Event? Event { get; set; } = null!;
    }
}
