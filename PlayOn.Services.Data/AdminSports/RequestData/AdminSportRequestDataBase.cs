using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    /// <summary>
    /// Base request data holding the fields and validation shared by creating and updating a sport.
    /// </summary>
    public abstract class AdminSportRequestDataBase : IValidatableObject
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        [EnumDataType(typeof(SportIcon))]
        public SportIcon Icon { get; set; }

        /// <summary>
        /// Default minimum number of players for matches of the sport.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int DefaultMinPlayers { get; set; }

        /// <summary>
        /// Default maximum number of players for matches of the sport.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int DefaultMaxPlayers { get; set; }

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the name must have enough characters once surrounding spaces are trimmed, and the minimum number of players cannot exceed the maximum.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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
