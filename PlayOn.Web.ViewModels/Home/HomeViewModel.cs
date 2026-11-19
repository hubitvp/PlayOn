using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel : IValidatableObject
    {
        public Guid? SportId { get; set; }
        public string? City { get; set; }
        public string? From { get; set; }

        public PageResult<HomeMatchCardViewModel>? Page { get; init; }
        public IEnumerable<SelectListItem> SportOptions { get; init; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SportId.HasValue && SportId.Value == Guid.Empty)
            {
                yield return new ValidationResult(
                    "Sport ID must be a not empty guid.",
                    new[] { nameof(SportId) });
            }

            if (!string.IsNullOrWhiteSpace(City) && City.Length > 100)
            {
                yield return new ValidationResult(
                    "City name cannot exceed 100 characters.",
                    new[] { nameof(City) });
            }

            if (!string.IsNullOrWhiteSpace(From))
            {
                if (!DateOnly.TryParse(
                    From,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedFrom))
                {
                    yield return new ValidationResult(
                        "Invalid date format.",
                        new[] { nameof(From) });
                }
            }
        }
    }
}