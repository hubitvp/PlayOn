using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Helpers;
using PlayOn.Services.Data.Attendance.RequestData;
using PlayOn.Services.Data.Attendance.ResponseModels;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;
using PlayOn.Web.DataBuilders.Formatting;
using PlayOn.Web.ViewModels.Match;
using System.Globalization;

namespace PlayOn.Web.DataBuilders;

public class MatchDataBuilder : DataBuilder
{
    private readonly IDisplayFormatter displayFormatter;
    private readonly SportDataBuilder sportDataBuilder;

    public MatchDataBuilder(
        SportDataBuilder sportDataBuilder,
        IDisplayFormatter displayFormatter)
    {
        this.sportDataBuilder = sportDataBuilder;
        this.displayFormatter = displayFormatter;
    }

    public MatchSearchRequestData
        MatchSearchToRequestData(
            MatchViewModel source)
    {
        var result = new MatchSearchRequestData
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
        };

        return result;
    }

    public MatchViewModel ToViewModel(
        MatchViewModel model,
        MatchResponseModel source)
    {
        return ToViewModel(model, source.Page, source.Options);
    }

    public MatchViewModel
        ToViewModel(
            MatchViewModel model,
            PageResult<MatchCardResponseModel>? page,
            MatchOptionsResponseModel optionsResponseModel)
    {
        PageResult<MatchCardViewModel>? pageViewModel = null;

        if (page != null)
        {
            pageViewModel = new PageResult<MatchCardViewModel>
            (
                pageItems: page.PageItems
                    .Select(MatchCardToViewModel)
                    .ToList(),
                pageMark: page.PageMark,
                pageSize: page.PageSize,
                totalItemsCount: page.TotalItemsCount
            );
        }

        IReadOnlyList<SelectListItem> sportOptions = optionsResponseModel.SportOptions
            .Select(x => SportOptionToSelectListItem(x, model.SportId))
            .ToList();

        IReadOnlyList<CourtOptionViewModel> courtOptions = optionsResponseModel.CourtOptions
            .Select(CourtOptionToViewModel)
            .ToList();

        IReadOnlyList<VenueOptionViewModel> venueOptions = optionsResponseModel.VenueOptions
            .Select(VenueOptionToViewModel)
            .ToList();


        model.SportOptions = sportOptions;
        model.CourtOptions = courtOptions;
        model.VenueOptions = venueOptions;
        model.Page = pageViewModel;

        return model;
    }

    public MatchCardViewModel
        MatchCardToViewModel(
            MatchCardResponseModel source)
    {
        var result = new MatchCardViewModel
        {
            Id = source.Id,
            Slug = Slug.Create(source.Title),
            Title = source.Title,
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            VenueName = source.VenueName,
            City = source.City,
            StartsAt = displayFormatter.ShortDateTime(source.Date.ToDateTime(source.StartsAt)),
            DurationMinutes = source.DurationMinutes,
            Joined = source.Joined,
            MaxPlayers = source.MaxPlayers,
            Waitlisted = source.Waitlisted,
            MinSkill = source.MinSkill,
            PricePerPlayer = displayFormatter.Price(source.PricePerPlayer),
            Status = source.Status
        };

        return result;
    }

    public MatchDetailsViewModel
        MatchDetailsToViewModel(
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
            StartsAt = displayFormatter.DateTime(start),
            EndsAt = displayFormatter.Time(start.AddMinutes(source.DurationMinutes)),
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
            PricePerPlayer = displayFormatter.Price(source.PricePerPlayer),
            OrganiserName = source.OrganiserName,
            OrganiserReliability = source.OrganiserReliability,
            CanManage = source.CanManage,
            CurrentUserStatus = source.CurrentUserStatus,
            CanJoin = source.CanJoin,
            CanLeave = source.CanLeave,
            CanTakeAttendance = source.CanTakeAttendance,
            CanRate = source.CanRate,
            Participants = source.Participants
                .Select(MatchParticipantToViewModel)
                .ToList()
        };

        return result;
    }

    public MatchParticipantViewModel
        MatchParticipantToViewModel(
            MatchParticipantResponseModel source)
    {
        var result = new MatchParticipantViewModel
        {
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            City = source.City,
            Status = source.Status,
            IsOrganiser = source.IsOrganiser,
            Reliability = source.Reliability
        };

        return result;
    }

    public MatchEditViewModel
        MatchEditToViewModel(
            MatchEditResponseModel source)
    {
        var result = new MatchEditViewModel
        {
            Id = source.Id,
            RowVersion = source.RowVersion,
            Title = source.Title,
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            SportName = source.SportName,
            SportIcon = source.SportIcon.ToCssClass(),
            CourtName = source.CourtName,
            CourtCapacity = source.CourtCapacity,
            VenueName = source.VenueName,
            City = source.City,
            StartsAt = displayFormatter.DateTime(source.Date.ToDateTime(source.StartsAt)),
            JoinedCount = source.JoinedCount
        };

        return result;
    }

    public MatchEditViewModel
        MatchEditApplyDisplayData(
            MatchEditViewModel target,
            MatchEditResponseModel source)
    {
        target.SportName = source.SportName;
        target.SportIcon = source.SportIcon.ToCssClass();
        target.CourtName = source.CourtName;
        target.CourtCapacity = source.CourtCapacity;
        target.VenueName = source.VenueName;
        target.City = source.City;
        target.StartsAt = displayFormatter.DateTime(source.Date.ToDateTime(source.StartsAt));
        target.JoinedCount = source.JoinedCount;

        return target;
    }

    public MatchUpdateRequestData
        MatchEditToRequestData(
            Guid matchId,
            Guid actorId,
            bool actorIsAdmin,
            MatchEditViewModel source)
    {
        var result = new MatchUpdateRequestData
        {
            MatchId = matchId,
            ActorId = actorId,
            ActorIsAdmin = actorIsAdmin,
            RowVersion = source.RowVersion,
            Title = source.Title,
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer
        };

        return result;
    }

    public SelectListItem
        SportOptionToSelectListItem(
            SportOptionResponseModel source,
            Guid? selectedSportId)
    {
        var target = new SelectListItem
        {
            Text = source.Name,
            Value = source.Id.ToString(),
            Selected = source.Id == selectedSportId
        };
        return target;
    }

    public CourtOptionViewModel
        CourtOptionToViewModel(
            CourtOptionResponseModel source)
    {
        var result = new CourtOptionViewModel
        {
            CourtId = source.CourtId,
            Name = source.Name,
            Surface = source.Surface.ToString(),
            Capacity = source.Capacity,
            SportIds = source.SportIds,
        };

        return result;
    }

    public VenueOptionViewModel
        VenueOptionToViewModel(
            VenueOptionResponseModel source)
    {
        var target = new VenueOptionViewModel
        {
            Id = source.Id,
            Label = $"{displayFormatter.ShortDate(source.Date)}, {displayFormatter.Time(source.StartsAt)} - {source.Name}, {source.City}",
            Date = displayFormatter.Date(source.Date),
            StartMinutes = source.StartsAt.Hour * 60 + source.StartsAt.Minute,
            CloseMinutes = source.ClosesAt.Hour * 60 + source.ClosesAt.Minute
        };

        return target;
    }

    public MatchCancelViewModel
        MatchCancelToViewModel(
            Guid matchId,
            MatchEditResponseModel source)
    {
        var result = new MatchCancelViewModel { MatchId = matchId };

        return MatchCancelApplyDisplayData(result, source);
    }

    public MatchCancelViewModel
        MatchCancelApplyDisplayData(
            MatchCancelViewModel target,
            MatchEditResponseModel source)
    {
        target.Title = source.Title;
        target.VenueName = source.VenueName;
        target.StartsAt = displayFormatter.DateTime(source.Date.ToDateTime(source.StartsAt));
        target.JoinedCount = source.JoinedCount;

        return target;
    }

    public MatchCancelRequestData
        MatchCancelToRequestData(
            Guid matchId,
            Guid actorId,
            bool actorIsAdmin,
            MatchCancelViewModel source)
    {
        var result = new MatchCancelRequestData
        {
            MatchId = matchId,
            ActorId = actorId,
            ActorIsAdmin = actorIsAdmin,
            Reason = source.Reason
        };

        return result;
    }

    public AttendanceViewModel
        AttendanceToViewModel(AttendanceResponseModel source)
    {
        var result = new AttendanceViewModel
        {
            MatchId = source.MatchId,
            Players = source.Players
                .Select(AttendancePlayerToViewModel)
                .ToList()
        };

        return AttendanceApplyDisplayData(result, source);
    }

    public AttendancePlayerInputModel
        AttendancePlayerToViewModel(AttendancePlayerResponseModel source)
    {
        var result = new AttendancePlayerInputModel
        {
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            City = source.City,
            Attended = true
        };

        return result;
    }

    public AttendanceViewModel
        AttendanceApplyDisplayData(
            AttendanceViewModel target,
            AttendanceResponseModel source)
    {
        target.Title = source.Title;
        target.VenueName = source.VenueName;
        target.StartsAt = displayFormatter.DateTime(source.Date.ToDateTime(source.StartsAt));

        foreach (var player in target.Players)
        {
            var match = source.Players.FirstOrDefault(x => x.UserId == player.UserId);

            if (match != null)
            {
                player.DisplayName = match.DisplayName;
                player.City = match.City;
            }
        }

        return target;
    }

    public AttendanceRequestData
        AttendanceToRequestData(
            Guid matchId,
            Guid actorId,
            bool actorIsAdmin,
            AttendanceViewModel source)
    {
        var result = new AttendanceRequestData
        {
            MatchId = matchId,
            ActorId = actorId,
            ActorIsAdmin = actorIsAdmin,
            Entries = source.Players
                .Select(x => new AttendanceEntryRequestData
                {
                    UserId = x.UserId,
                    Attended = x.Attended
                })
                .ToList()
        };

        return result;
    }

    public MatchFormViewModel
    MatchFormToViewModel(
        Guid? venueId,
        MatchOptionsResponseModel options)
    {
        var result = new MatchFormViewModel();

        if (venueId.HasValue)
        {
            result.VenueId = venueId.Value;
        }

        return MatchFormOptionsToViewModel(result, options);
    }

    public MatchFormViewModel
        MatchFormOptionsToViewModel(
            MatchFormViewModel target,
            MatchOptionsResponseModel options)
    {
        target.Sports = options.SportOptions
            .Select(x => SportOptionToSelectListItem(x, target.SportId))
            .ToList();

        target.Courts = options.CourtOptions
            .Select(CourtOptionToViewModel)
            .ToList();

        target.Slots = options.VenueOptions
            .Select(VenueOptionToViewModel)
            .ToList();

        return target;
    }

    public MatchCreateRequestData
        MatchFormToRequestData(
            Guid organiserId,
            MatchFormViewModel source)
    {
        var result = new MatchCreateRequestData
        {
            OrganiserId = organiserId,
            SportId = source.SportId ?? Guid.Empty,
            CourtId = source.CourtId ?? Guid.Empty,
            VenueId = source.VenueId ?? Guid.Empty,
            Title = source.Title,
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer
        };

        return result;
    }
}