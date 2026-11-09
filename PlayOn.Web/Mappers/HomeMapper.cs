using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Web.ViewModels.Home;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Mappers;

public static class HomeMapper
{
    public static HomeQueryRequestData
        ToHomeQuery(
            HomeSearchInputModel source,
            int page)
    {
        var result = new HomeQueryRequestData
        {
            SportId = source.SportId,
            City = source.City,
            From = source.From != null ? DateOnly.Parse(source.From) : null,
            Page = page,
            PageSize = DefaultPageSize
        };

        return result;
    }

    public static MatchCardViewModel
        ToMatchCardViewModel(
            MatchCardResponseModel source)
    {
        var result = new MatchCardViewModel
        {
            Id = source.Id,
            Title = source.Title,
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            VenueName = source.VenueName,
            City = source.City,
            StartsAt = source.Date
                .ToDateTime(source.StartsAt, DateTimeKind.Local)
                .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture),
            DurationMinutes = source.DurationMinutes,
            Joined = source.Joined,
            MaxPlayers = source.MaxPlayers,
            Waitlisted = source.Waitlisted,
            MinSkill = source.MinSkill,
            PricePerPlayer = source.PricePerPlayer.ToFormattedString(),
            Status = source.Status,
            Slug = string.Empty
        };

        return result;
    }

    public static SelectListItem
        ToSportSelectListItem(
            SportsResponseModel source,
            int? selectedSportId)
    {
        var result = new SelectListItem
        {
            Text = source.Name,
            Value = source.Id.ToString(),
            Selected = source.Id == selectedSportId
        };

        return result;
    }
}
