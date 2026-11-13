using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    public class SportPreference
    {
        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public Guid SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public Sport Sport { get; set; } = null!;


        public SkillLevel SkillLevel { get; set; }

    }
}
