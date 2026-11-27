using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminSports;
using PlayOn.Services.Data.AdminSports.RequestData;
using PlayOn.Services.Data.AdminSports.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds sport projections, response models and entities for administration.
/// </summary>
public static class AdminSportDataBuilder
{
    /// <summary>
    /// Creates a query projection from a sport to a row with usage counts.
    /// </summary>
    /// <returns>An expression that projects a sport to an admin sport row.</returns>
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

    /// <summary>
    /// Maps a sport row to a list response model.
    /// </summary>
    /// <param name="source">The sport row.</param>
    /// <returns>The sport response model.</returns>
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

    /// <summary>
    /// Maps a sport entity to an edit response model.
    /// </summary>
    /// <param name="source">The sport entity.</param>
    /// <returns>The sport edit response model.</returns>
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

    /// <summary>
    /// Creates a new sport entity from the create request.
    /// </summary>
    /// <param name="source">The create request data.</param>
    /// <returns>A new sport populated from the request.</returns>
    public static Sport ToEntity(AdminSportCreateRequestData source)
    {
        var result = new Sport();

        ApplyToEntity(source, result);

        return result;
    }

    /// <summary>
    /// Copies the request values onto a sport entity, trimming the name.
    /// </summary>
    /// <param name="source">The request data.</param>
    /// <param name="target">The sport entity to update.</param>
    public static void ApplyToEntity(AdminSportRequestDataBase source, Sport target)
    {
        target.Name = source.Name.Trim();
        target.Icon = source.Icon;
        target.DefaultMinPlayers = source.DefaultMinPlayers;
        target.DefaultMaxPlayers = source.DefaultMaxPlayers;
    }
}
