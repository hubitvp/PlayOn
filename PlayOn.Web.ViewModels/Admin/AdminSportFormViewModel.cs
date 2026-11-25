using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminSportFormViewModel : IValidatableObject
    {
        [ValidateNever]
        public Guid? Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        [EnumDataType(typeof(SportIcon))]
        [Display(Name = "Icon")]
        public SportIcon Icon { get; set; } = SportIcon.TrophyDefault;

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        [Display(Name = "Default minimum players")]
        public int DefaultMinPlayers { get; set; } = 4;

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        [Display(Name = "Default maximum players")]
        public int DefaultMaxPlayers { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((Name ?? string.Empty).Trim().Length < NameMinLength)
            {
                yield return new ValidationResult(
                    $"Name must have at least {NameMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Name) });
            }

            if (DefaultMinPlayers > DefaultMaxPlayers)
            {
                yield return new ValidationResult(
                    "Minimum players cannot be more than maximum players.",
                    new[] { nameof(DefaultMinPlayers) });
            }
        }
    }
}