using PlayOn.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    public class SportPreference
    {
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public Sport Sport { get; set; } = null!;


        public SkillLevel SkillLevel { get; set; }

    }
}
