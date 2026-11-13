using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchSearchInputModel : IValidatableObject
    {

        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? SearchTerm { get; set; }


        public Guid? SportId { get; set; }

        [StringLength(MatchRules.CityMaxLength)]
        public string? City { get; set; }

        public string? From { get; set; }

        [EnumDataType(
            typeof(SkillLevel),
            ErrorMessage = "Select a valid skill level.")]
        public SkillLevel? Skill { get; set; }

        public bool OnlyWithFreeSpots { get; set; }

        [EnumDataType(
            typeof(MatchSort),
            ErrorMessage = "Select a valid sort order.")]
        public MatchSort Sort { get; set; } = MatchSort.Soonest;

        public int Page { get; set; } = FirstPage;

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

        private static DateOnly GetLocalDate(ValidationContext validationContext)
        {
            var timeProvider =
                validationContext.GetService(typeof(TimeProvider)) as TimeProvider
                ?? TimeProvider.System;

            return DateOnly.FromDateTime(timeProvider.GetLocalNow().LocalDateTime);
        }
    }
}
