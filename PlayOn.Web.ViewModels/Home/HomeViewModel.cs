using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel : IValidatableObject
    {
        [FromQuery(Name = "sport")]
        public Guid? SportId { get; set; }

        [FromQuery(Name = "city")]
        public string? City { get; set; }

        [FromQuery(Name = "from")]
        public string? From { get; set; }

        public PageResult<HomeMatchCardViewModel>? Page { get; set; }
        public IEnumerable<SelectListItem> SportOptions { get; set; } = Enumerable.Empty<SelectListItem>();

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