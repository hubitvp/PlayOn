using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Services.Data.Match.RequestData
{

    /// <summary>
    /// Base request data holding the fields and validation shared by creating and updating a match.
    /// </summary>
    public abstract class MatchRequestDataBase : IValidatableObject
    {
        /// <summary>
        /// Title of the match.
        /// </summary>
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Optional description of the match.
        /// </summary>
        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; }

        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MinSkill { get; set; }

        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MaxSkill { get; set; }

        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }


        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the title must have enough characters once surrounding spaces are trimmed, minimum players and skill cannot exceed their maximums, and the duration must be a multiple of the allowed step.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Trim().Length < TitleMinLength)
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
                    "Minimum skill cannot be higher than maximum skill.",
                    new[] { nameof(MinSkill) });
            }

            if (DurationMinutes % MatchRules.DurationStepInMinutes != 0)
            {
                yield return new ValidationResult(
                    $"Duration must be a multiple of {MatchRules.DurationStepInMinutes} minutes.",
                    new[] { nameof(DurationMinutes) });
            }
        }
    }
}
