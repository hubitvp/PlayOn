using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    public abstract class AdminVenueRequestDataBase : IValidatableObject
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartsAt { get; set; }

        public TimeOnly OpensAt { get; set; }

        public TimeOnly ClosesAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Trim().Length < NameMinLength)
            {
                yield return new ValidationResult(
                    $"The name must have at least {NameMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Name) });
            }

            if (Address.Trim().Length < AddressMinLength)
            {
                yield return new ValidationResult(
                    $"The address must have at least {AddressMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Address) });
            }

            if (City.Trim().Length < CityMinLength)
            {
                yield return new ValidationResult(
                    $"The city must have at least {CityMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(City) });
            }

            if (OpensAt >= ClosesAt)
            {
                yield return new ValidationResult(
                    "The venue must open before it closes.",
                    new[] { nameof(OpensAt) });
            }

            if (StartsAt < OpensAt || StartsAt >= ClosesAt)
            {
                yield return new ValidationResult(
                    "The slot must start within the opening hours.",
                    new[] { nameof(StartsAt) });
            }
        }
    }
}
