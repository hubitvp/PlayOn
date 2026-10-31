using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;


namespace PlayOn.Data.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = string.Empty;



        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = string.Empty;


        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; } = string.Empty;

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }


        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly MatchStartsAt { get; set; }

        [Required]
        public TimeOnly OpensAt { get; set; }

        [Required]
        public TimeOnly ClosesAt { get; set; }

        [MaxLength(PhotoUrlMaxLength)]
        public string? PhotoUrl { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        public virtual Event? Event { get; set; } = null!;
    }
}
