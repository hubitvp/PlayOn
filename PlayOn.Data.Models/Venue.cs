using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a venue (sports facility) where events take place.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        [Key]
        [Comment("Unique identifier of the venue")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Name of the venue")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Street address of the venue.
        /// </summary>
        [Required]
        [MaxLength(AddressMaxLength)]
        [Comment("Street address of the venue")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        [Required]
        [MaxLength(CityMaxLength)]
        [Comment("City in which the venue is located")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Geographic latitude of the venue.
        /// </summary>
        [Required]
        [Comment("Geographic latitude of the venue")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Geographic longitude of the venue.
        /// </summary>
        [Required]
        [Comment("Geographic longitude of the venue")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Date of the event held at the venue.
        /// </summary>
        [Required]
        [Comment("Date of the event held at the venue")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Time the event starts.
        /// </summary>
        [Required]
        [Comment("Time the event starts")]
        public TimeOnly StartsAt { get; set; }

        /// <summary>
        /// Time the venue opens.
        /// </summary>
        [Required]
        [Comment("Time the venue opens")]
        public TimeOnly OpensAt { get; set; }

        /// <summary>
        /// Time the venue closes.
        /// </summary>
        [Required]
        [Comment("Time the venue closes")]
        public TimeOnly ClosesAt { get; set; }

        /// <summary>
        /// Soft-delete flag indicating whether the venue has been deleted.
        /// </summary>
        [Required]
        [Comment("Soft-delete flag indicating whether the venue has been deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The event held at this venue.
        /// </summary>
        public virtual Event? Event { get; set; } = null!;
    }
}
