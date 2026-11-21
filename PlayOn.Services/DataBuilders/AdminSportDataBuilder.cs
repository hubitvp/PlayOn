using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminSports;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class AdminSportDataBuilder
{
    internal static Expression<Func<Sport, AdminSportRow>>
        SportProjection()
    {
        return source => new AdminSportRow
        {
            Id = source.Id,
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers,
            CourtsCount = source.Courts.Count,
            MatchesCount = source.Events.Count,
            PreferencesCount = source.Preferences.Count
        };
    }

    internal static AdminSportResponseModel
        SportToResponseModel(AdminSportRow source)
    {
        var result = new AdminSportResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers,
            CourtsCount = source.CourtsCount,
            MatchesCount = source.MatchesCount,
            PreferencesCount = source.PreferencesCount
        };

        return result;
    }

    public static AdminSportEditResponseModel
        SportToEditResponseModel(Sport source)
    {
        var result = new AdminSportEditResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            Icon = source.Icon,
            DefaultMinPlayers = source.DefaultMinPlayers,
            DefaultMaxPlayers = source.DefaultMaxPlayers
        };

        return result;
    }

    public static Sport ToEntity(AdminSportCreateRequestData source)
    {
        var result = new Sport();

        ApplyToEntity(source, result);

        return result;
    }

    public static void ApplyToEntity(AdminSportRequestDataBase source, Sport target)
    {
        target.Name = source.Name.Trim();
        target.Icon = source.Icon;
        target.DefaultMinPlayers = source.DefaultMinPlayers;
        target.DefaultMaxPlayers = source.DefaultMaxPlayers;
    }
}
