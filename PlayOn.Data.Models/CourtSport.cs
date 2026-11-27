using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayOn.Data.Models
{
    /// <summary>
    /// Mapping entity describing which sports can be played on which court (many-to-many).
    /// </summary>
    public class CourtSport
    {
        /// <summary>
        /// Identifier of the court.
        /// </summary>
        [Required]
        [Comment("Identifier of the court")]
        public Guid CourtId { get; set; }

        /// <summary>
        /// The court the sport can be played on.
        /// </summary>
        [ForeignKey(nameof(CourtId))]
        public virtual Court Court { get; set; } = null!;

        /// <summary>
        /// Identifier of the sport.
        /// </summary>
        [Required]
        [Comment("Identifier of the sport")]
        public Guid SportId { get; set; }

        /// <summary>
        /// The sport that can be played on the court.
        /// </summary>
        [ForeignKey(nameof(SportId))]
        public virtual Sport Sport { get; set; } = null!;

        /// <summary>
        /// Collection of events played with this court and sport combination.
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
