using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Input model of the form for creating a match.
    /// </summary>
    public class MatchCreateInputModel : IValidatableObject
    {

        /// <summary>
        /// Identifier of the sport to be played.
        /// </summary>
        [Required(ErrorMessage = "Select a sport.")]
        public Guid SportId { get; set; }

        /// <summary>
        /// Identifier of the court where the match is played.
        /// </summary>
        [Required(ErrorMessage = "Select a court.")]
        public Guid CourtId { get; set; }

        /// <summary>
        /// Identifier of the venue slot booked for the match.
        /// </summary>
        [Required(ErrorMessage = "Select a venue slot.")]
        public Guid VenueId { get; set; }

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
        [Required]
        [Range(MatchRules.MinDurationInMinutes, MatchRules.MaxDurationInMinutes)]
        [DurationStep]
        public int DurationMinutes { get; set; } = MatchRules.MinDurationInMinutes;

        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        [Required]
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MinPlayers { get; set; } = MatchRules.PlayersMinLimit;

        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        [Required]
        [Range(MatchRules.PlayersMinLimit, MatchRules.PlayersMaxLimit)]
        public int MaxPlayers { get; set; } = MatchRules.PlayersMaxLimit;

        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        [Required]
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MinSkill { get; set; } = SkillLevel.Beginner;

        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        [Required]
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel MaxSkill { get; set; } = SkillLevel.Advanced;

        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        [Required]
        [Range(typeof(decimal), MatchRules.MinPriceToDecimal, MatchRules.MaxPriceToDecimal,
            ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
        public decimal PricePerPlayer { get; set; }


        /// <summary>
        /// Sports available for selection.
        /// </summary>
        public IReadOnlyList<SelectListItem> SportOptions { get; set; } = [];
        /// <summary>
        /// Courts available for selection.
        /// </summary>
        public IReadOnlyList<CourtOptionViewModel> CourtOptions { get; set; } = [];
        /// <summary>
        /// Venue slots available for selection.
        /// </summary>
        public IReadOnlyList<VenueOptionViewModel> VenueOptions { get; set; } = [];



        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the minimum number of players cannot exceed the maximum, and the minimum skill cannot be higher than the maximum skill.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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

        }
    }
}