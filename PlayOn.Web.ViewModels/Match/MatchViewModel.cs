using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// View model of the match search page: the filters, the matches found and the available options.
    /// </summary>
    public class MatchViewModel : IValidatableObject
    {
        /// <summary>
        /// Optional search term used to filter the matches (query parameter <c>term</c>).
        /// </summary>
        [FromQuery(Name = "term")]
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? SearchTerm { get; set; }


        /// <summary>
        /// Optional sport used to filter the matches (query parameter <c>sport</c>).
        /// </summary>
        [FromQuery(Name = "sport")]
        public Guid? SportId { get; set; }

        /// <summary>
        /// Optional city used to filter the matches (query parameter <c>city</c>).
        /// </summary>
        [FromQuery(Name = "city")]
        [StringLength(MatchRules.CityMaxLength)]
        public string? City { get; set; }

        /// <summary>
        /// Optional date, as text, from which matches are shown; it cannot be in the past (query parameter <c>from</c>).
        /// </summary>
        [FromQuery(Name = "from")]
        public string? From { get; set; }


        /// <summary>
        /// Optional skill level used to filter the matches (query parameter <c>skill</c>).
        /// </summary>
        [FromQuery(Name = "skill")]
        [EnumDataType(
            typeof(SkillLevel),
            ErrorMessage = "Select a valid skill level.")]
        public SkillLevel? Skill { get; set; }


        /// <summary>
        /// When <c>true</c>, only matches that still have free spots are shown (query parameter <c>free</c>).
        /// </summary>
        [FromQuery(Name = "free")]
        public bool OnlyWithFreeSpots { get; set; }


        /// <summary>
        /// Order in which the matches are shown (query parameter <c>sort</c>).
        /// </summary>
        [FromQuery(Name = "sort")]
        [EnumDataType(
            typeof(MatchSort),
            ErrorMessage = "Select a valid sort order.")]
        public MatchSort Sort { get; set; } = MatchSort.Soonest;


        /// <summary>
        /// Sports available in the filter.
        /// </summary>
        public IReadOnlyList<SelectListItem>? SportOptions { get; set; }
        /// <summary>
        /// Courts available for selection.
        /// </summary>
        public IReadOnlyList<CourtOptionViewModel>? CourtOptions { get; set; }
        /// <summary>
        /// Venue slots available for selection.
        /// </summary>
        public IReadOnlyList<VenueOptionViewModel>? VenueOptions { get; set; }

        /// <summary>
        /// Page of match cards found.
        /// </summary>
        public PageResult<MatchCardViewModel>? Page { get; set; }


        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the date, if given, must be valid and cannot be in the past.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(From))
            {
                if (!DateOnly.TryParse(
                    From,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
                {
                    yield return new ValidationResult(
                        "Enter a valid date.",
                        new[] { nameof(From) });
                }
                else if (date < GetLocalDate(validationContext))
                {
                    yield return new ValidationResult(
                        "The date cannot be in the past.",
                        new[] { nameof(From) });
                }
            }
        }

        /// <summary>
        /// Gets the current local date, using the <see cref="TimeProvider"/> registered in the validation services or the system time provider when none is registered.
        /// </summary>
        /// <param name="validationContext">The context of the validation, used to resolve the time provider.</param>
        /// <returns>The current local date.</returns>
        private static DateOnly GetLocalDate(ValidationContext validationContext)
        {
            var timeProvider =
                validationContext.GetService(typeof(TimeProvider)) as TimeProvider
                ?? TimeProvider.System;

            return DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);
        }
    }
}