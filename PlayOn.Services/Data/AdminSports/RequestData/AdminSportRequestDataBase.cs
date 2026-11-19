using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    public abstract class AdminSportRequestDataBase : IValidatableObject
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [EnumDataType(typeof(SportIcon))]
        public SportIcon Icon { get; set; }

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int DefaultMinPlayers { get; set; }

        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int DefaultMaxPlayers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Trim().Length < NameMinLength)
            {
                yield return new ValidationResult(
                    $"The name must have at least {NameMinLength} characters, not counting spaces at the ends.",
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
