using Microsoft.EntityFrameworkCore;
using PlayOn.Common.EntityConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a playable court available for booking.
    /// </summary>
    public class Court
    {
        /// <summary>
        /// Unique identifier of the court.
        /// </summary>
        [Key]
        [Comment("Unique identifier of the court")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the court.
        /// </summary>
        [Required]
        [MaxLength(CourtConstants.NameMaxLength)]
        [Comment("Name of the court")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        [Required]
        [Comment("Type of playing surface of the court")]
        public Surface Surface { get; set; }

        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        [Required]
        [Comment("Maximum number of people the court can accommodate")]
        public int Capacity { get; set; }

        /// <summary>
        /// Price for renting the court for one hour.
        /// </summary>
        [Required]
        [Comment("Price for renting the court for one hour")]
        public decimal HourlyPrice { get; set; }

        /// <summary>
        /// Collection of sports that can be played on this court.
        /// </summary>
        public virtual ICollection<CourtSport> Sports { get; set; }
            = new List<CourtSport>();

        /// <summary>
        /// Collection of events scheduled on this court.
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
