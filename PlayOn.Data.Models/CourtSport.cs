using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayOn.Data.Models
{

    public class CourtSport
    {
        [Required]
        public int CourtId { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; } = null!;

        [Required]
        public int SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public virtual Sport Sport { get; set; } = null!;
    }
}
