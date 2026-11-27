using PlayOn.Data.Models;
using PlayOn.Services.Data.Players;
using PlayOn.Services.Data.Players.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds projections, entities and response models for players.
/// </summary>
public static class PlayerDataBuilder
{
    /// <summary>
    /// Creates a projection from a user to the basic player row.
    /// </summary>
    /// <returns>The projection expression.</returns>
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

    /// <summary>
    /// Creates a projection from a sport preference to a player skill model.
    /// </summary>
    /// <returns>The projection expression.</returns>
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

    /// <summary>
    /// Creates a projection from a rating to a rating model with rater name, score, comment and match title.
    /// </summary>
    /// <returns>The projection expression.</returns>
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

    /// <summary>
    /// Creates the player profile response model; the average rating is null when there are no ratings.
    /// </summary>
    /// <param name="source">The player row.</param>
    /// <param name="attended">The number of attended matches.</param>
    /// <param name="noShow">The number of no-shows.</param>
    /// <param name="lateCancel">The number of late cancellations.</param>
    /// <param name="ratingCount">The number of visible ratings.</param>
    /// <param name="ratingSum">The sum of visible rating scores.</param>
    /// <param name="skills">The player's skills.</param>
    /// <param name="recentRatings">The recent rated comments.</param>
    /// <param name="isOwnProfile">Whether the viewer is the profile owner.</param>
    /// <returns>The profile response model.</returns>
    internal static PlayerProfileResponseModel
        PlayerToProfileResponseModel(
            PlayerRow source,
            int attended,
            int noShow,
            int lateCancel,
            int ratingCount,
            int ratingSum,
            IReadOnlyList<PlayerSkillResponseModel> skills,
            IReadOnlyList<PlayerRatingResponseModel> recentRatings,
            bool isOwnProfile)
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
            IsOwnProfile = isOwnProfile,
            Skills = skills,
            RecentRatings = recentRatings
        };

        return result;
    }

    /// <summary>
    /// Creates the profile edit response model.
    /// </summary>
    /// <param name="source">The player row.</param>
    /// <param name="skills">The skill options for every sport.</param>
    /// <returns>The edit response model.</returns>
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

    /// <summary>
    /// Creates a sport preference entity for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="sportId">The identifier of the sport.</param>
    /// <param name="skillLevel">The skill level.</param>
    /// <returns>The new sport preference entity.</returns>
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
