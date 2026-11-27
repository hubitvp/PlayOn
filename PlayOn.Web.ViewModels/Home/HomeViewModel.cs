using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PlayOn.Web.ViewModels.Home
{
    /// <summary>
    /// View model of the home page: the match filters, the matches found and the available sport options.
    /// </summary>
    public class HomeViewModel : IValidatableObject
    {
        /// <summary>
        /// Optional sport used to filter the matches (query parameter <c>sport</c>).
        /// </summary>
        [FromQuery(Name = "sport")]
        public Guid? SportId { get; set; }

        /// <summary>
        /// Optional city used to filter the matches (query parameter <c>city</c>).
        /// </summary>
        [FromQuery(Name = "city")]
        public string? City { get; set; }

        /// <summary>
        /// Optional date, as text, from which matches are shown (query parameter <c>from</c>).
        /// </summary>
        [FromQuery(Name = "from")]
        public string? From { get; set; }

        /// <summary>
        /// Page of match cards found.
        /// </summary>
        public PageResult<HomeMatchCardViewModel>? Page { get; set; }
        /// <summary>
        /// Sports available in the filter.
        /// </summary>
        public IEnumerable<SelectListItem> SportOptions { get; set; } = Enumerable.Empty<SelectListItem>();

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: a selected sport cannot be an empty identifier, the city cannot exceed 100 characters, and the date must be in a valid format.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SportId.HasValue && SportId.Value == Guid.Empty)
            {
                yield return new ValidationResult(
                    "Select a valid sport.",
                    new[] { nameof(SportId) });
            }

            if (!string.IsNullOrWhiteSpace(City) && City.Length > 100)
            {
                yield return new ValidationResult(
                    "City name cannot exceed 100 characters.",
                    new[] { nameof(City) });
            }

            if (!string.IsNullOrWhiteSpace(From) &&
                !DateOnly.TryParse(
                    From,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _))
            {
                yield return new ValidationResult(
                    "Invalid date format.",
                    new[] { nameof(From) });
            }
        }
    }
}