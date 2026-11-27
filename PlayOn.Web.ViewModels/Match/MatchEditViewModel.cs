using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Read-only match data displayed next to the edit form.
    /// </summary>
    public class EditDisplayData
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string? SportName { get; set; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public string? SportIcon { get; set; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public string? CourtName { get; set; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public int? CourtCapacity { get; set; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string? VenueName { get; set; }
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Formatted date and time the match starts.
        /// </summary>
        public string? StartsAt { get; set; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public int? JoinedCount { get; set; }
    }

    /// <summary>
    /// Model of the form for editing a match.
    /// </summary>
    public class MatchEditViewModel : IValidatableObject
    {
        /// <summary>
        /// Identifier of the match being edited.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// Concurrency token received when the match was loaded, used to detect conflicting edits.
        /// </summary>
        [MinLength(1, ErrorMessage = "This page is out of date. Open the match again.")]
        public byte[] RowVersion { get; set; } = [];

        /// <summary>
        /// Title of the match.
        /// </summary>
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the match.
        /// </summary>
        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        [DurationStep]
        public int DurationMinutes { get; set; } = MatchRules.MinDurationInMinutes;

        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; } = MatchRules.PlayersMinLimit;

        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; } = MatchRules.PlayersMaxLimit;

        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        [EnumDataType(typeof(SkillLevel), ErrorMessage = "Select a valid skill level.")]
        public SkillLevel MinSkill { get; set; } = SkillLevel.Beginner;

        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        [EnumDataType(typeof(SkillLevel), ErrorMessage = "Select a valid skill level.")]
        public SkillLevel MaxSkill { get; set; } = SkillLevel.Advanced;

        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }

        /// <summary>
        /// Read-only match data displayed next to the form.
        /// </summary>
        public EditDisplayData? EditDisplayData { get; set; }

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the title must have enough characters once surrounding spaces are trimmed, the minimum number of players cannot exceed the maximum, and the minimum skill cannot be higher than the maximum skill.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var trimmedTitleLength = (Title ?? string.Empty).Trim().Length;

            if (trimmedTitleLength < TitleMinLength)
            {
                yield return new ValidationResult(
                    $"The title must have at least {TitleMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Title) });
            }

            if (MinPlayers > MaxPlayers)
            {
                yield return new ValidationResult(
                    "Minimum players cannot be more than maximum players.",
                    new[] { nameof(MinPlayers) });
            }

            if (MinSkill > MaxSkill)
            {
                yield return new ValidationResult(
                    "The lowest skill cannot be higher than the highest skill.",
                    new[] { nameof(MinSkill) });
            }
        }
    }
}