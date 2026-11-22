using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchEditViewModel : IValidatableObject
    {
        [Required]
        public Guid MatchId { get; set; }


        [MinLength(1, ErrorMessage = "This page is out of date. Open the match again.")]
        public byte[] RowVersion { get; set; } = [];


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



        public string? SportName { get; set; }
        public string? SportIcon { get; set; }
        public string? CourtName { get; set; }
        public int? CourtCapacity { get; set; }
        public string? VenueName { get; set; }
        public string? City { get; set; }
        public string? StartsAt { get; set; }
        public int? JoinedCount { get; set; }



        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
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