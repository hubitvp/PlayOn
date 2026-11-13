using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayOn.Data.Models
{

    public class CourtSport
    {
        [Required]
        public Guid CourtId { get; set; }

        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; } = null!;

        [Required]
        public Guid SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public virtual Sport Sport { get; set; } = null!;


        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
