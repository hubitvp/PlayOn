using PlayOn.Data.Models;
using PlayOn.Services.Data.Players;
using PlayOn.Services.Data.Players.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class PlayerDataBuilder
{
    internal static Expression<Func<ApplicationUser, PlayerRow>>
        PlayerRowProjection()
    {
        return source => new PlayerRow
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            City = source.City,
            IsActive = source.IsActive,
            ReliabilityScore = source.ReliabilityScore,
            MatchesPlayed = source.MatchesPlayed
        };
    }

    public static Expression<Func<SportPreference, PlayerSkillResponseModel>>
        PlayerSkillProjection()
    {
        return source => new PlayerSkillResponseModel
        {
            SportName = source.Sport.Name,
            SportIcon = source.Sport.Icon,
            SkillLevel = source.SkillLevel
        };
    }

    public static Expression<Func<PlayerRating, PlayerRatingResponseModel>>
        PlayerRatingProjection()
    {
        return source => new PlayerRatingResponseModel
        {
            RaterName = source.Rater.DisplayName,
            Score = source.Score,
            Comment = source.Comment,
            MatchTitle = source.Match.Title
        };
    }

    internal static PlayerProfileResponseModel
        PlayerToProfileResponseModel(
            PlayerRow source,
            int attended,
            int noShow,
            int lateCancel,
            int ratingCount,
            int ratingSum,
            IReadOnlyList<PlayerSkillResponseModel> skills,
            IReadOnlyList<PlayerRatingResponseModel> recentRatings)
    {
        var result = new PlayerProfileResponseModel
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            City = source.City,
            ReliabilityScore = source.ReliabilityScore,
            MatchesPlayed = source.MatchesPlayed,
            AttendedCount = attended,
            NoShowCount = noShow,
            LateCancelCount = lateCancel,
            RatingCount = ratingCount,
            AverageRating = ratingCount == 0 ? null : (double)ratingSum / ratingCount,
            Skills = skills,
            RecentRatings = recentRatings
        };

        return result;
    }

    internal static PlayerEditResponseModel
        PlayerToEditResponseModel(
            PlayerRow source,
            IReadOnlyList<PlayerSkillOptionResponseModel> skills)
    {
        var result = new PlayerEditResponseModel
        {
            UserId = source.Id,
            DisplayName = source.DisplayName,
            City = source.City,
            Skills = skills
        };

        return result;
    }

    public static SportPreference
        SkillToEntity(
            Guid userId,
            Guid sportId,
            PlayOn.Common.Enums.SkillLevel skillLevel)
    {
        var result = new SportPreference
        {
            UserId = userId,
            SportId = sportId,
            SkillLevel = skillLevel
        };

        return result;
    }
}
