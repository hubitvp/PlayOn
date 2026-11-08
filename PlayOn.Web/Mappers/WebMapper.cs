using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.RequestData;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Services.Data.Venue.ResponseModels;
using PlayOn.Web.ViewModels.Home;
using PlayOn.Web.ViewModels.Match;
using PlayOn.Web.ViewModels.Venue;
using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Mappers;

[Mapper]
public partial class WebMapper
{
    public HomeQueryRequestData ToHomeQuery(HomeSearchInputModel source, int page)
        => new()
        {
            SportId = source.SportId,
            City = source.City,
            From = source.From != null ? DateOnly.Parse(source.From) : null,
            Page = page,
            PageSize = DefaultPageSize
        };

    [MapperIgnoreTarget(nameof(MatchListViewModel.Sports))]
    [MapperIgnoreTarget(nameof(MatchListViewModel.Matches))]
    [MapperIgnoreTarget(nameof(MatchListViewModel.Page))]
    [MapperIgnoreTarget(nameof(MatchListViewModel.PageSize))]
    [MapperIgnoreTarget(nameof(MatchListViewModel.TotalPages))]
    [MapperIgnoreTarget(nameof(MatchListViewModel.TotalItems))]
    [MapperIgnoreSource(nameof(MatchListInputModel.Page))]
    public partial MatchListViewModel MapMatchListCore(
        MatchListInputModel source);

    public MatchListViewModel ToMatchListViewModel(
        MatchListInputModel source,
        PagedResult<MatchCardResponseModel>? page,
        IEnumerable<SportsResponseModel> sports)
    {
        var target = MapMatchListCore(source);

        return new MatchListViewModel
        {
            SearchTerm = target.SearchTerm,
            SportId = target.SportId,
            City = target.City,
            From = target.From,
            Skill = target.Skill,
            OnlyWithFreeSpots = target.OnlyWithFreeSpots,
            Sort = target.Sort,
            Sports = sports
                .Select(x => ToSportSelectListItem(x, source.SportId))
                .ToList(),
            Matches = page?.Items
                .Select(ToMatchCardViewModel)
                .Select(x =>
                {
                    x.Slug = SlugHelper.Create(x.Title);
                    return x;
                })
                .ToList() ?? [],
            Page = page?.Page ?? FirstPage,
            PageSize = page?.PageSize ?? DefaultPageSize,
            TotalPages = page?.TotalPages ?? 0,
            TotalItems = page?.TotalCount ?? 0
        };
    }

    [MapperIgnoreTarget(nameof(MatchCardViewModel.Slug))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.SportIcon))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.StartsAt))]
    [MapperIgnoreTarget(nameof(MatchCardViewModel.PricePerPlayer))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.SportIcon))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.Date))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.StartsAt))]
    [MapperIgnoreSource(nameof(MatchCardResponseModel.PricePerPlayer))]
    public partial MatchCardViewModel MapMatchCardCore(MatchCardResponseModel source);

    public MatchCardViewModel ToMatchCardViewModel(
        MatchCardResponseModel source)
    {
        var target = MapMatchCardCore(source);
        target.SportIcon = source.SportIcon.ToCssClass();
        target.StartsAt = source.Date
            .ToDateTime(source.StartsAt, DateTimeKind.Local)
            .ToString("dddd, dd MMMM  HH:mm", CultureInfo.InvariantCulture);
        target.PricePerPlayer = source.PricePerPlayer.ToFormattedString();
        return target;
    }

    [MapperIgnoreTarget(nameof(MatchDetailsViewModel.Slug))]
    [MapProperty(nameof(MatchDetailsResponseModel.SportIcon), nameof(MatchDetailsViewModel.SportIcon), Use = nameof(MapSportIcon))]
    [MapperIgnoreTarget(nameof(MatchDetailsViewModel.StartsAt))]
    [MapperIgnoreTarget(nameof(MatchDetailsViewModel.EndsAt))]
    [MapProperty(nameof(MatchDetailsResponseModel.PricePerPlayer), nameof(MatchDetailsViewModel.PricePerPlayer), Use = nameof(MapPrice))]
    public MatchDetailsViewModel MapMatchDetailsCore(
        MatchDetailsResponseModel source)
        => new()
        {
            Id = source.Id,
            Slug = string.Empty,
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            CancelReason = source.CancelReason,
            IsHidden = source.IsHidden,
            SportName = source.SportName,
            SportIcon = MapSportIcon(source.SportIcon),
            StartsAt = string.Empty,
            EndsAt = string.Empty,
            DurationMinutes = source.DurationMinutes,
            VenueName = source.VenueName,
            Address = source.Address,
            City = source.City,
            CourtName = source.CourtName,
            Surface = source.Surface,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            Joined = source.Joined,
            Waitlisted = source.Waitlisted,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = MapPrice(source.PricePerPlayer),
            OrganiserName = source.OrganiserName,
            OrganiserReliability = source.OrganiserReliability,
            CanManage = source.CanManage,
            Participants = source.Participants.Select(MapMatchParticipant).ToList()
        };

    public MatchDetailsViewModel ToMatchDetailsViewModel(
        MatchDetailsResponseModel source,
        string slug)
    {
        var target = MapMatchDetailsCore(source);
        var start = source.Date.ToDateTime(source.StartsAt);

        return new MatchDetailsViewModel
        {
            Id = target.Id,
            Slug = slug,
            Title = target.Title,
            Description = target.Description,
            Status = target.Status,
            CancelReason = target.CancelReason,
            IsHidden = target.IsHidden,
            SportName = target.SportName,
            SportIcon = target.SportIcon,
            StartsAt = start.ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture),
            EndsAt = start.AddMinutes(source.DurationMinutes).ToString("HH:mm", CultureInfo.InvariantCulture),
            DurationMinutes = target.DurationMinutes,
            VenueName = target.VenueName,
            Address = target.Address,
            City = target.City,
            CourtName = target.CourtName,
            Surface = target.Surface,
            MinPlayers = target.MinPlayers,
            MaxPlayers = target.MaxPlayers,
            Joined = target.Joined,
            Waitlisted = target.Waitlisted,
            MinSkill = target.MinSkill,
            MaxSkill = target.MaxSkill,
            PricePerPlayer = target.PricePerPlayer,
            OrganiserName = target.OrganiserName,
            OrganiserReliability = target.OrganiserReliability,
            CanManage = target.CanManage,
            Participants = target.Participants
        };
    }

    [MapperIgnoreSource(nameof(MatchParticipantResponseModel.UserId))]
    public MatchParticipantViewModel MapMatchParticipant(
        MatchParticipantResponseModel source)
        => new()
        {
            DisplayName = source.DisplayName,
            City = source.City,
            Status = source.Status,
            IsOrganiser = source.IsOrganiser,
            Reliability = source.Reliability
        };

    [MapperIgnoreTarget(nameof(SelectListItem.Selected))]
    public SelectListItem MapSportSelectListItemCore(
        SportsResponseModel source)
        => new()
        {
            Text = source.Name,
            Value = source.Id.ToString()
        };

    public SelectListItem ToSportSelectListItem(
        SportsResponseModel source,
        int? selectedSportId)
    {
        var target = MapSportSelectListItemCore(source);
        target.Selected = source.Id == selectedSportId;
        return target;
    }

    private static string MapSportIcon(PlayOn.Common.Enums.SportIcon source) => source.ToCssClass();

    private static string MapPrice(decimal source) => source.ToFormattedString();

    [MapperIgnoreTarget(nameof(CourtViewModel.SportIconCss))]
    [MapperIgnoreTarget(nameof(CourtViewModel.Surface))]
    [MapperIgnoreTarget(nameof(CourtViewModel.HourlyPrice))]
    [MapperIgnoreSource(nameof(CourtResponseModel.SportIcon))]
    [MapperIgnoreSource(nameof(CourtResponseModel.Surface))]
    [MapperIgnoreSource(nameof(CourtResponseModel.HourlyPrice))]
    public partial CourtViewModel MapCourtCore(CourtResponseModel source);

    public CourtViewModel ToCourtViewModel(CourtResponseModel source)
    {
        var target = MapCourtCore(source);
        target.SportIconCss = source.SportIcon.ToCssClass();
        target.Surface = source.Surface.ToString();
        target.HourlyPrice = source.HourlyPrice.ToFormattedString();
        return target;
    }

    [MapperIgnoreTarget(nameof(VenueViewModelData.Court))]
    [MapperIgnoreSource(nameof(VenueResponseModel.Court))]
    [MapperIgnoreTarget(nameof(VenueViewModelData.MatchSlug))]
    public partial VenueViewModelData MapVenueCore(VenueResponseModel source);

    public VenueViewModelData ToVenueViewModel(VenueResponseModel source)
    {
        var target = MapVenueCore(source);
        target.Court = ToCourtViewModel(source.Court);
        return target;
    }

    [MapperIgnoreTarget(nameof(MatchListRequestData.From))]
    [MapperIgnoreSource(nameof(MatchListInputModel.From))]
    [MapperIgnoreTarget(nameof(MatchListRequestData.PageSize))]
    [MapperIgnoreTarget(nameof(MatchListRequestData.Page))]
    [MapperIgnoreSource(nameof(MatchListInputModel.Page))]
    public partial MatchListRequestData MapMatchListRequestData(MatchListInputModel source);


    public MatchListRequestData ToMatchListRequestData(MatchListInputModel source)
    {

        var target = MapMatchListRequestData(source);
        target.From =
            DateOnly.TryParse(
                source.From,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date)
                    ? date
                    : null;
        target.Page = Math.Max(FirstPage, source.Page);
        target.PageSize = DefaultPageSize;

        return target;

    }


}
