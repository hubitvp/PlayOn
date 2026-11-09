using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches.RequestData;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.Mappers;

public static class MatchesMapper
{
    public static MatchListRequestData
        ToMatchListRequestData(
            MatchListInputModel source)
    {
        var result = new MatchListRequestData
        {
            SearchTerm = source.SearchTerm,
            SportId = source.SportId,
            City = source.City,
            From = DateOnly.TryParse(
                source.From,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date)
                ? date
                : null,
            Skill = source.Skill,
            OnlyWithFreeSpots = source.OnlyWithFreeSpots,
            Sort = source.Sort,
            Page = Math.Max(FirstPage, source.Page),
            PageSize = DefaultPageSize
        };

        return result;
    }

    public static MatchListViewModel
        ToMatchListViewModel(
            MatchListInputModel source,
            PagedResult<MatchCardResponseModel>? page,
            IEnumerable<SportsResponseModel> sports)
    {
        var result = new MatchListViewModel
        {
            SearchTerm = source.SearchTerm,
            SportId = source.SportId,
            City = source.City,
            From = source.From,
            Skill = source.Skill,
            OnlyWithFreeSpots = source.OnlyWithFreeSpots,
            Sort = source.Sort,
            Sports = sports
                .Select(x => HomeMapper.ToSportSelectListItem(x, source.SportId))
                .ToList(),
            Matches = page?.Items
                .Select(HomeMapper.ToMatchCardViewModel)
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

        return result;
    }

    public static MatchDetailsViewModel
        ToMatchDetailsViewModel(
            MatchDetailsResponseModel source,
            string slug)
    {
        var start = source.Date.ToDateTime(source.StartsAt);

        var result = new MatchDetailsViewModel
        {
            Id = source.Id,
            Slug = slug,
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            CancelReason = source.CancelReason,
            IsHidden = source.IsHidden,
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            StartsAt = start.ToString("dddd, dd MMMM yyyy, HH:mm", CultureInfo.InvariantCulture),
            EndsAt = start.AddMinutes(source.DurationMinutes).ToString("HH:mm", CultureInfo.InvariantCulture),
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
            PricePerPlayer = source.PricePerPlayer.ToFormattedString(),
            OrganiserName = source.OrganiserName,
            OrganiserReliability = source.OrganiserReliability,
            CanManage = source.CanManage,
            Participants = source.Participants
                .Select(ToMatchParticipantViewModel)
                .ToList()
        };

        return result;
    }

    public static MatchParticipantViewModel
        ToMatchParticipantViewModel(
            MatchParticipantResponseModel source)
    {
        var result = new MatchParticipantViewModel
        {
            DisplayName = source.DisplayName,
            City = source.City,
            Status = source.Status,
            IsOrganiser = source.IsOrganiser,
            Reliability = source.Reliability
        };

        return result;
    }
}
