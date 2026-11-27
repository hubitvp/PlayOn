using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// Form model used to create or edit a sport in the administration area.
    /// </summary>
    public class AdminSportFormViewModel : IValidatableObject
    {
        /// <summary>
        /// Identifier of the sport being edited; <c>null</c> when a new sport is being created.
        /// </summary>
        [ValidateNever]
        public Guid? Id { get; set; }

        /// <summary>
        /// Name of the sport.
        /// </summary>
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        [EnumDataType(typeof(SportIcon))]
        [Display(Name = "Icon")]
        public SportIcon Icon { get; set; } = SportIcon.TrophyDefault;

        /// <summary>
        /// Default minimum number of players for matches of the sport.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        [Display(Name = "Default minimum players")]
        public int DefaultMinPlayers { get; set; } = 4;

        /// <summary>
        /// Default maximum number of players for matches of the sport.
        /// </summary>
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        [Display(Name = "Default maximum players")]
        public int DefaultMaxPlayers { get; set; } = 10;

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the name must have enough characters once surrounding spaces are trimmed, and the minimum number of players cannot exceed the maximum.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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