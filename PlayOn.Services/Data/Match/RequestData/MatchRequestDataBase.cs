using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Services.Data.Match.RequestData
{

    public abstract class MatchRequestDataBase : IValidatableObject
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        public int DurationMinutes { get; set; }

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; }

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; }

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MinSkill { get; set; }

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MaxSkill { get; set; }

        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }


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
