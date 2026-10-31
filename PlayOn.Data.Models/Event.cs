using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayOn.Data.Models
{
    public class Event
    {
        [Required]
        public int MatchId { get; set; }

        [ForeignKey(nameof(MatchId))]
        public Match Match { get; set; } = null!;


        [Required]
        public int SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public Sport Sport { get; set; } = null!;


        [Required]
        public int CourtId { get; set; }

        [ForeignKey(nameof(CourtId))]
        public Court Court { get; set; } = null!;



        [Required]
        public int VenueId { get; set; }

        [ForeignKey(nameof(VenueId))]
        public Venue Venue { get; set; } = null!;
    }
}
