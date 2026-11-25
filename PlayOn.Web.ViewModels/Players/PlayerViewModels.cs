using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Enums;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Web.ViewModels.Players
{
    public class PlayerProfileViewModel
    {
        public required Guid Id { get; init; }
        public required string DisplayName { get; init; }
        public required string City { get; init; }
        public int? Reliability { get; init; }
        public required int MatchesPlayed { get; init; }
        public required int AttendedCount { get; init; }
        public required int NoShowCount { get; init; }
        public required int LateCancelCount { get; init; }
        public required int RatingCount { get; init; }
        public string? AverageRating { get; init; }
        public required bool IsOwnProfile { get; init; }
        public required IReadOnlyList<PlayerSkillViewModel> Skills { get; init; }
        public required IReadOnlyList<PlayerRatingViewModel> RecentRatings { get; init; }
    }

    public class PlayerSkillViewModel
    {
        public required string SportName { get; init; }
        public required string SportIcon { get; init; }
        public required SkillLevel SkillLevel { get; init; }
    }

    public class PlayerRatingViewModel
    {
        public required string RaterName { get; init; }
        public required int Score { get; init; }
        public string? Comment { get; init; }
        public required string MatchTitle { get; init; }
    }

    public class ProfileEditViewModel : IValidatableObject
    {
        [Required]
        [StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        public List<PlayerSkillInputModel> Skills { get; set; } = [];

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

    public class PlayerSkillInputModel
    {
        [NotEmptyGuid(ErrorMessage = "The sport is not valid.")]
        public Guid SportId { get; set; }

        [ValidateNever]
        public string SportName { get; set; } = null!;

        [EnumDataType(typeof(SkillLevel), ErrorMessage = "Select a valid skill level.")]
        public SkillLevel? SkillLevel { get; set; }
    }
}