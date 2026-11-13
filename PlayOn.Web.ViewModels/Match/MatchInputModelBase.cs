using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{

    public abstract class MatchInputModelBase : IValidatableObject
    {
        [Required]
        [StringLength(
            TitleMaxLength,
            MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(
            MatchRules.MinDurationInMinutes,
            MatchRules.MaxDurationInMinutes)]
        public int DurationMinutes { get; set; } = MatchRules.MinDurationInMinutes;

        [Range(
            MatchRules.PlayersMinLimit,
            MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; } = MatchRules.PlayersMinLimit;

        [Range(
            MatchRules.PlayersMinLimit,
            MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; } = MatchRules.PlayersMaxLimit;

        [EnumDataType(
            typeof(SkillLevel),
            ErrorMessage = "Select a valid skill level.")]
        public SkillLevel MinSkill { get; set; } = SkillLevel.Beginner;

        [EnumDataType(
            typeof(SkillLevel),
            ErrorMessage = "Select a valid skill level.")]
        public SkillLevel MaxSkill { get; set; } = SkillLevel.Advanced;

        [Range(
            typeof(decimal),
            MatchRules.MinPriceToDecimal,
            MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true,
            ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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

            if (DurationMinutes % MatchRules.DurationStepInMinutes != 0)
            {
                yield return new ValidationResult(
                    $"Duration must be a multiple of {MatchRules.DurationStepInMinutes} minutes.",
                    new[] { nameof(DurationMinutes) });
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
