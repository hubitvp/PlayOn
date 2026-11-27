using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Enums;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Web.ViewModels.Players
{
    /// <summary>
    /// View model of the public profile and statistics of a player.
    /// </summary>
    public class PlayerProfileViewModel
    {
        /// <summary>
        /// Unique identifier of the player.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Display name of the player.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// City of the player.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Reliability score of the player, if one has been calculated.
        /// </summary>
        public int? Reliability { get; init; }
        /// <summary>
        /// Total number of matches the player has played.
        /// </summary>
        public required int MatchesPlayed { get; init; }
        /// <summary>
        /// Number of matches the player attended.
        /// </summary>
        public required int AttendedCount { get; init; }
        /// <summary>
        /// Number of matches the player did not show up for.
        /// </summary>
        public required int NoShowCount { get; init; }
        /// <summary>
        /// Number of times the player cancelled too close to the start of a match.
        /// </summary>
        public required int LateCancelCount { get; init; }
        /// <summary>
        /// Number of ratings received by the player.
        /// </summary>
        public required int RatingCount { get; init; }
        /// <summary>
        /// Formatted average score of the ratings received by the player, if any.
        /// </summary>
        public string? AverageRating { get; init; }
        /// <summary>
        /// Indicates whether the profile belongs to the current user.
        /// </summary>
        public required bool IsOwnProfile { get; init; }
        /// <summary>
        /// Skill levels of the player per sport.
        /// </summary>
        public required IReadOnlyList<PlayerSkillViewModel> Skills { get; init; }
        /// <summary>
        /// Most recent ratings received by the player.
        /// </summary>
        public required IReadOnlyList<PlayerRatingViewModel> RecentRatings { get; init; }
    }

    /// <summary>
    /// A skill level of a player in a sport, shown on the player profile.
    /// </summary>
    public class PlayerSkillViewModel
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required string SportIcon { get; init; }
        /// <summary>
        /// Skill level of the player in the sport.
        /// </summary>
        public required SkillLevel SkillLevel { get; init; }
    }

    /// <summary>
    /// A rating received by a player, shown on the player profile.
    /// </summary>
    public class PlayerRatingViewModel
    {
        /// <summary>
        /// Display name of the user who gave the rating.
        /// </summary>
        public required string RaterName { get; init; }
        /// <summary>
        /// Score given by the rater.
        /// </summary>
        public required int Score { get; init; }
        /// <summary>
        /// Optional comment given by the rater.
        /// </summary>
        public string? Comment { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string MatchTitle { get; init; }
    }

    /// <summary>
    /// Model of the form for editing the player's own profile.
    /// </summary>
    public class ProfileEditViewModel : IValidatableObject
    {
        /// <summary>
        /// Display name of the player.
        /// </summary>
        [Required]
        [StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// City of the player.
        /// </summary>
        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        /// <summary>
        /// Skill levels of the player per sport.
        /// </summary>
        public List<PlayerSkillInputModel> Skills { get; set; } = [];

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the display name and city must have enough characters once surrounding spaces are trimmed.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((DisplayName ?? string.Empty).Trim().Length < DisplayNameMinLength)
            {
                yield return new ValidationResult(
                    $"Display name must have at least {DisplayNameMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(DisplayName) });
            }

            if ((City ?? string.Empty).Trim().Length < CityMinLength)
            {
                yield return new ValidationResult(
                    $"City must have at least {CityMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(City) });
            }
        }
    }

    /// <summary>
    /// The skill level of a player in a single sport on the profile edit form.
    /// </summary>
    public class PlayerSkillInputModel
    {
        /// <summary>
        /// Identifier of the sport.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The sport is not valid.")]
        public Guid SportId { get; set; }

        /// <summary>
        /// Name of the sport.
        /// </summary>
        [ValidateNever]
        public string SportName { get; set; } = null!;

        /// <summary>
        /// Skill level of the player in the sport, or <c>null</c> if not set.
        /// </summary>
        [EnumDataType(typeof(SkillLevel), ErrorMessage = "Select a valid skill level.")]
        public SkillLevel? SkillLevel { get; set; }
    }
}