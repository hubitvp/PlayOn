using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// Form model used to create or edit a venue (and its slot) in the administration area.
    /// </summary>
    public class AdminVenueFormViewModel : IValidatableObject
    {
        /// <summary>
        /// Identifier of the venue being edited; <c>null</c> when a new venue is being created.
        /// </summary>
        [ValidateNever]
        public Guid? Id { get; set; }

        /// <summary>
        /// Indicates whether the venue slot is already booked for a match.
        /// </summary>
        [ValidateNever]
        public bool IsBooked { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Street address of the venue.
        /// </summary>
        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Geographic latitude of the venue, between -90 and 90.
        /// </summary>
        [Range(-90, 90)]
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Geographic longitude of the venue, between -180 and 180.
        /// </summary>
        [Range(-180, 180)]
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Time the slot starts; must fall within the opening hours.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Slot starts at")]
        public TimeOnly StartsAt { get; set; }

        /// <summary>
        /// Time the venue opens.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Venue opens at")]
        public TimeOnly OpensAt { get; set; }

        /// <summary>
        /// Time the venue closes.
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Venue closes at")]
        public TimeOnly ClosesAt { get; set; }


        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the name, address and city must have enough characters once surrounding spaces are trimmed, the venue must open before it closes, and the slot must start within the opening hours.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((Name ?? string.Empty).Trim().Length < NameMinLength)
            {
                yield return new ValidationResult(
                    $"Name must have at least {NameMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Name) });
            }

            if ((Address ?? string.Empty).Trim().Length < AddressMinLength)
            {
                yield return new ValidationResult(
                    $"Address must have at least {AddressMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Address) });
            }

            if ((City ?? string.Empty).Trim().Length < CityMinLength)
            {
                yield return new ValidationResult(
                    $"City must have at least {CityMinLength} characters, not counting spaces at the ends.",
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