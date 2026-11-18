using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchFormViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Select a sport.")]
        public Guid? SportId { get; set; }

        [Required(ErrorMessage = "Select a court.")]
        public Guid? CourtId { get; set; }

        [Required(ErrorMessage = "Select a date and time slot.")]
        public Guid? VenueId { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        public int DurationMinutes { get; set; } = 60;

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; } = 2;

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; } = 10;

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MinSkill { get; set; } = SkillLevel.Beginner;

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MaxSkill { get; set; } = SkillLevel.Advanced;

        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }

        public IReadOnlyList<SelectListItem> Sports { get; set; } = [];
        public IReadOnlyList<CourtOptionViewModel> Courts { get; set; } = [];
        public IReadOnlyList<VenueOptionViewModel> Slots { get; set; } = [];

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Title) && Title.Trim().Length < TitleMinLength)
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