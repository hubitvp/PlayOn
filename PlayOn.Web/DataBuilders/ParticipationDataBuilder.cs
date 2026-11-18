using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Participation.RequestData;
using PlayOn.Services.Data.Participation.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Participation;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.DataBuilders;

public class ParticipationDataBuilder
{
    private readonly IDisplayFormatter displayFormatter;

    public ParticipationDataBuilder(IDisplayFormatter displayFormatter)
    {
        this.displayFormatter = displayFormatter;
    }


    public ParticipationRequestData
        ParticipationToRequestData(
            Guid matchId,
            Guid userId)
    {
        var result = new ParticipationRequestData
        {
            MatchId = matchId,
            UserId = userId
        };

        return result;
    }

    public MyMatchesRequestData
        MyMatchesToRequestData(
            Guid userId,
            MyMatchesTab tab,
            int page)
    {
        var result = new MyMatchesRequestData
        {
            UserId = userId,
            Tab = tab,
            Page = page,
            PageSize = DefaultPageSize
        };

        return result;
    }

    public MyMatchesViewModel
        MyMatchesToViewModel(
            MyMatchesTab tab,
            PageResult<MyMatchResponseModel>? page,
            int requestedPage)
    {
        var result = new MyMatchesViewModel
        {
            Tab = tab,
            Matches = page?.PageItems
                .Select(x => MyMatchToViewModel(tab, x))
                .ToList() ?? [],
            Page = page?.PageMark ?? requestedPage,
            PageSize = page?.PageSize ?? DefaultPageSize,
            TotalPages = page?.TotalPages ?? 0
        };

        return result;
    }

    public MyMatchViewModel
        MyMatchToViewModel(
            MyMatchesTab tab,
            MyMatchResponseModel source)
    {
        var canLeave = (tab == MyMatchesTab.Upcoming || tab == MyMatchesTab.Waitlist)
            && !source.IsOrganiser;

        var result = new MyMatchViewModel
        {
            Id = source.Id,
            Slug = Slug.Create(source.Title),
            Title = source.Title,
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            VenueName = source.VenueName,
            City = source.City,
            StartsAt = displayFormatter.ShortDateTime(source.Date.ToDateTime(source.StartsAt)),
            Status = source.Status,
            MatchStatus = source.MatchStatus,
            WaitlistPosition = source.WaitlistPosition,
            CanLeave = canLeave,
            CanRate = source.CanRate
        };

        return result;
    }

    public string JoinToMessage(JoinMatchResponseModel source)
    {
        if (source.Status == ParticipantStatus.Waitlisted)
        {
            return $"The match is full. You are number {source.WaitlistPosition} on the waiting list.";
        }

        return source.MatchConfirmed
            ? "You joined the match. It now has enough players and is confirmed."
            : "You joined the match.";
    }

    public string LeaveToMessage(LeaveMatchResponseModel source)
    {
        return source.IsLateCancel
            ? "You left the match. Leaving this late counts as a strike."
            : "You left the match.";
    }
}
