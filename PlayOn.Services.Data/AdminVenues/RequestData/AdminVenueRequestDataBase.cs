using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    /// <summary>
    /// Base request data holding the fields and validation shared by creating and updating a venue.
    /// </summary>
    public abstract class AdminVenueRequestDataBase : IValidatableObject
    {
        /// <summary>
        /// Name of the venue.
        /// </summary>
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Street address of the venue.
        /// </summary>
        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        /// <summary>
        /// Geographic latitude of the venue, between -90 and 90.
        /// </summary>
        [Range(-90, 90)]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Geographic longitude of the venue, between -180 and 180.
        /// </summary>
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Date of the slot offered by the venue.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Time the slot starts; must fall within the opening hours.
        /// </summary>
        public TimeOnly StartsAt { get; set; }

        /// <summary>
        /// Time the venue opens.
        /// </summary>
        public TimeOnly OpensAt { get; set; }

        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public TimeOnly ClosesAt { get; set; }

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the name, address and city must have enough characters once surrounding spaces are trimmed, the venue must open before it closes, and the slot must start within the opening hours.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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
