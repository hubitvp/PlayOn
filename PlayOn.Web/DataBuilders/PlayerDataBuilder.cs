using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Players.RequestData;
using PlayOn.Services.Data.Players.ResponseModels;
using PlayOn.Web.ViewModels.Players;
using System.Globalization;

namespace PlayOn.Web.DataBuilders;

public class PlayerDataBuilder : DataBuilder
{
    public PlayerProfileViewModel
        PlayerProfileToViewModel(
            PlayerProfileResponseModel source)
    {
        var result = new PlayerProfileViewModel
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            City = source.City,
            Reliability = source.ReliabilityScore,
            MatchesPlayed = source.MatchesPlayed,
            AttendedCount = source.AttendedCount,
            NoShowCount = source.NoShowCount,
            LateCancelCount = source.LateCancelCount,
            RatingCount = source.RatingCount,
            AverageRating = source.AverageRating?.ToString("0.0", CultureInfo.InvariantCulture),
            IsOwnProfile = source.IsOwnProfile,
            Skills = source.Skills
                .Select(PlayerSkillToViewModel)
                .ToList(),
            RecentRatings = source.RecentRatings
                .Select(PlayerRatingToViewModel)
                .ToList()
        };

        return result;
    }

    public PlayerSkillViewModel
        PlayerSkillToViewModel(PlayerSkillResponseModel source)
    {
        var result = new PlayerSkillViewModel
        {
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            SkillLevel = source.SkillLevel
        };

        return result;
    }

    public PlayerRatingViewModel
        PlayerRatingToViewModel(PlayerRatingResponseModel source)
    {
        var result = new PlayerRatingViewModel
        {
            RaterName = source.RaterName,
            Score = source.Score,
            Comment = source.Comment,
            MatchTitle = source.MatchTitle
        };

        return result;
    }

    public ProfileEditViewModel
        PlayerEditToViewModel(PlayerEditResponseModel source)
    {
        var result = new ProfileEditViewModel
        {
            DisplayName = source.DisplayName,
            City = source.City,
            Skills = source.Skills
                .Select(PlayerSkillOptionToViewModel)
                .ToList()
        };

        return result;
    }

    public PlayerSkillInputModel
        PlayerSkillOptionToViewModel(PlayerSkillOptionResponseModel source)
    {
        var result = new PlayerSkillInputModel
        {
            SportId = source.SportId,
            SportName = source.SportName,
            SkillLevel = source.SkillLevel
        };

        return result;
    }

    /// <summary>
    /// Refills the sport names of a posted profile form so it can be shown again with the typed values.
    /// </summary>
    public ProfileEditViewModel
        PlayerEditApplyDisplayData(
            ProfileEditViewModel target,
            PlayerEditResponseModel source)
    {
        foreach (var skill in target.Skills)
        {
            var match = source.Skills.SingleOrDefault(x => x.SportId == skill.SportId);

            if (match != null)
            {
                skill.SportName = match.SportName;
            }
        }

        return target;
    }

    public PlayerUpdateRequestData
        PlayerEditToRequestData(
            Guid userId,
            ProfileEditViewModel source)
    {
        var result = new PlayerUpdateRequestData
        {
            UserId = userId,
            DisplayName = source.DisplayName,
            City = source.City,
            Skills = source.Skills
                .Select(x => new PlayerSkillRequestData
                {
                    SportId = x.SportId,
                    SkillLevel = x.SkillLevel
                })
                .ToList()
        };

        return result;
    }
}
