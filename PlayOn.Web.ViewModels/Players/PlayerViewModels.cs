using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Enums;
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

    public class ProfileEditViewModel
    {
        [Required]
        [StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = string.Empty;

        public List<PlayerSkillInputModel> Skills { get; set; } = [];
    }

    public class PlayerSkillInputModel
    {
        public Guid SportId { get; set; }

        [ValidateNever]
        public string SportName { get; set; } = string.Empty;

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel? SkillLevel { get; set; }
    }
}
