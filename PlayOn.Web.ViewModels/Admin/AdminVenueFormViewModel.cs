using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminVenueFormViewModel : IValidatableObject
    {
        [ValidateNever]
        public Guid? Id { get; set; }

        [ValidateNever]
        public bool IsBooked { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Range(-90, 90)]
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }

        [Range(-180, 180)]
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Slot starts at")]
        public TimeOnly StartsAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Venue opens at")]
        public TimeOnly OpensAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Venue closes at")]
        public TimeOnly ClosesAt { get; set; }

        [StringLength(PhotoUrlMaxLength)]
        [Display(Name = "Photo address (optional)")]
        public string? PhotoUrl { get; set; }

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