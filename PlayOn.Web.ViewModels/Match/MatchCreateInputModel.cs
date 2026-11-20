using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchCreateInputModel : IValidatableObject
    {

        [Required(ErrorMessage = "Select a sport.")]
        public Guid SportId { get; set; }

        [Required(ErrorMessage = "Select a court.")]
        public Guid CourtId { get; set; }

        [Required(ErrorMessage = "Select a venue slot.")]
        public Guid VenueId { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        public int DurationMinutes { get; set; } = MatchRules.MinDurationInMinutes;

        [Required]
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; } = MatchRules.PlayersMinLimit;

        [Required]
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; } = MatchRules.PlayersMaxLimit;

        [Required]
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MinSkill { get; set; } = SkillLevel.Beginner;

        [Required]
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MaxSkill { get; set; } = SkillLevel.Advanced;

        [Required]
        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }


        public IReadOnlyList<SelectListItem> SportOptions { get; set; } = [];
        public IReadOnlyList<CourtOptionViewModel> CourtOptions { get; set; } = [];
        public IReadOnlyList<VenueOptionViewModel> VenueOptions { get; set; } = [];



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
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