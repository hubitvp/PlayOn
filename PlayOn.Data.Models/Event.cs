using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents the scheduled occurrence of a match, tying it to a sport, court and venue.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Identifier of the match this event belongs to.
        /// </summary>
        [Required]
        [Comment("Identifier of the match this event belongs to")]
        public Guid MatchId { get; set; }

        /// <summary>
        /// The match this event belongs to.
        /// </summary>
        [ForeignKey(nameof(MatchId))]
        public Match Match { get; set; } = null!;

        /// <summary>
        /// Identifier of the sport being played.
        /// </summary>
        [Required]
        [Comment("Identifier of the sport being played")]
        public Guid SportId { get; set; }

        /// <summary>
        /// The sport being played.
        /// </summary>
        [ForeignKey(nameof(SportId))]
        public Sport Sport { get; set; } = null!;

        /// <summary>
        /// Identifier of the court where the event takes place.
        /// </summary>
        [Required]
        [Comment("Identifier of the court where the event takes place")]
        public Guid CourtId { get; set; }

        /// <summary>
        /// The court where the event takes place.
        /// </summary>
        [ForeignKey(nameof(CourtId))]
        public Court Court { get; set; } = null!;

        /// <summary>
        /// Identifier of the venue where the event takes place.
        /// </summary>
        [Required]
        [Comment("Identifier of the venue where the event takes place")]
        public Guid VenueId { get; set; }

        /// <summary>
        /// The venue where the event takes place.
        /// </summary>
        [ForeignKey(nameof(VenueId))]
        public Venue Venue { get; set; } = null!;

        /// <summary>
        /// The court and sport combination used by this event.
        /// </summary>
        public virtual CourtSport CourtSport { get; set; } = null!;
    }
}
