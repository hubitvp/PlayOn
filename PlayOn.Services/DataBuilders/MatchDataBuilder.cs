using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Match;
using PlayOn.Services.Data.Match.RequestData;
using PlayOn.Services.Data.Match.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

internal static class MatchDataBuilder
{
    internal static Expression<Func<Match, DetailsRow>> MatchDetailsRowProjection()
    {
        return match => new DetailsRow
        {
            Id = match.Id,
            Title = match.Title,
            Description = match.Description,
            Status = match.Status,
            CancelReason = match.CancelReason,
            DurationMinutes = match.DurationMinutes,
            MinPlayers = match.MinPlayers,
            MaxPlayers = match.MaxPlayers,
            MinSkill = match.MinSkill,
            MaxSkill = match.MaxSkill,
            PricePerPlayer = match.PricePerPlayer,
            OrganiserId = match.OrganiserId,
            OrganiserName = match.Organiser.DisplayName,
            OrganiserReliability = match.Organiser.ReliabilityScore,
            SportName = match.Event!.Sport.Name,
            SportIcon = match.Event.Sport.Icon,
            CourtName = match.Event.Court.Name,
            Surface = match.Event.Court.Surface,
            CourtCapacity = match.Event.Court.Capacity,
            VenueName = match.Event.Venue.Name,
            Address = match.Event.Venue.Address,
            City = match.Event.Venue.City,
            Date = match.Event.Venue.Date,
            StartsAt = match.Event.Venue.StartsAt,
            ClosesAt = match.Event.Venue.ClosesAt,
            Participants = match.Participants
                .Where(participant => participant.Status != ParticipantStatus.Cancelled)
                .OrderBy(participant => participant.JoinedOn)
                .ThenBy(participant => participant.MatchId)
                .ThenBy(participant => participant.UserId)
                .Select(participant => new ParticipantRow
                {
                    UserId = participant.UserId,
                    DisplayName = participant.User.DisplayName,
                    City = participant.User.City,
                    Status = participant.Status,
                    Reliability = participant.User.ReliabilityScore
                })
                .ToList()
        };
    }

    internal static Expression<Func<Match, MatchCardResponseModel>> MatchCardProjection()
    {
        return source => new MatchCardResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            SportName = source.Event!.Sport.Name,
            SportIcon = source.Event.Sport.Icon,
            VenueName = source.Event.Venue.Name,
            City = source.Event.Venue.City,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt,
            DurationMinutes = source.DurationMinutes,
            Joined = source.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            MaxPlayers = source.MaxPlayers,
            Waitlisted = source.Participants.Count(x => x.Status == ParticipantStatus.Waitlisted),
            MinSkill = source.MinSkill,
            PricePerPlayer = source.PricePerPlayer,
            Status = source.Status
        };
    }

    internal static MatchDetailsResponseModel MatchDetailsRowToResponseModel(
        DetailsRow source,
        Guid? userId,
        bool isManager,
        bool isOpenForChanges,
        bool hasEnded)
    {
        var currentStatus = userId == null
            ? (ParticipantStatus?)null
            : source.Participants
                .Where(x => x.UserId == userId)
                .Select(x => (ParticipantStatus?)x.Status)
                .SingleOrDefault();

        var isOrganiser = userId != null && userId == source.OrganiserId;

        var isActive = currentStatus == ParticipantStatus.Joined
            || currentStatus == ParticipantStatus.Waitlisted;

        return new MatchDetailsResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            CancelReason = source.CancelReason,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            OrganiserName = source.OrganiserName,
            OrganiserReliability = source.OrganiserReliability,
            SportName = source.SportName,
            SportIcon = source.SportIcon,
            CourtName = source.CourtName,
            Surface = source.Surface,
            CourtCapacity = source.CourtCapacity,
            VenueName = source.VenueName,
            Address = source.Address,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            ClosesAt = source.ClosesAt,
            Joined = source.Participants.Count(x =>
                x.Status == ParticipantStatus.Joined
                || x.Status == ParticipantStatus.Attended
                || x.Status == ParticipantStatus.NoShow),
            Waitlisted = source.Participants.Count(x => x.Status == ParticipantStatus.Waitlisted),
            CanManage = isManager && isOpenForChanges,
            CurrentUserStatus = currentStatus,
            CanJoin = userId != null && isOpenForChanges && currentStatus == null,
            CanLeave = isOpenForChanges && isActive && !isOrganiser,
            CanTakeAttendance = isManager
                && hasEnded
                && (source.Status == MatchStatus.Open || source.Status == MatchStatus.Confirmed),
            CanRate = source.Status == MatchStatus.Completed
                && currentStatus == ParticipantStatus.Attended,
            Participants = source.Participants
                .Select(x => new MatchParticipantResponseModel
                {
                    UserId = x.UserId,
                    DisplayName = x.DisplayName,
                    City = x.City,
                    Status = x.Status,
                    IsOrganiser = x.UserId == source.OrganiserId,
                    Reliability = x.Reliability
                })
                .ToList()
        };
    }


    internal static Expression<Func<Match, MatchEditDisplayDataResponseModel>> MatchEditDisplayDataProjection()
    {
        return source => new MatchEditDisplayDataResponseModel
        {
            SportName = source.Event!.Sport.Name,
            SportIcon = source.Event.Sport.Icon,
            CourtName = source.Event.Court.Name,
            CourtCapacity = source.Event!.Court.Capacity,
            VenueName = source.Event.Venue.Name,
            City = source.Event.Venue.City,
            Date = source.Event.Venue.Date,
            StartsAt = source.Event.Venue.StartsAt,
            ClosesAt = source.Event.Venue.ClosesAt,
            JoinedCount = source.Participants.Count(x => x.Status == ParticipantStatus.Joined)
        };

    }



    internal static Expression<Func<Match, EditRow>> MatchEditRowProjection()
    {
        return source => new EditRow
        {
            Id = source.Id,
            OrganiserId = source.OrganiserId,
            IsDeleted = source.IsDeleted,
            Status = source.Status,
            Title = source.Title,
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            RowVersion = source.RowVersion,

            EditRowDisplayData = new EditRowDisplayData
            {
                SportName = source.Event!.Sport.Name,
                SportIcon = source.Event.Sport.Icon,
                CourtId = source.Event.CourtId,
                CourtName = source.Event.Court.Name,
                CourtCapacity = source.Event!.Court.Capacity,
                VenueName = source.Event.Venue.Name,
                City = source.Event.Venue.City,
                Date = source.Event.Venue.Date,
                StartsAt = source.Event.Venue.StartsAt,
                ClosesAt = source.Event.Venue.ClosesAt,
                JoinedCount = source.Participants.Count(x => x.Status == ParticipantStatus.Joined)
            }
        };
    }

    internal static MatchEditDisplayDataResponseModel
        MatchEditRowToDisplayDataResponseModel(EditRow source)
    {
        return new MatchEditDisplayDataResponseModel
        {
            SportName = source.EditRowDisplayData.SportName,
            SportIcon = source.EditRowDisplayData.SportIcon,
            CourtName = source.EditRowDisplayData.CourtName,
            CourtCapacity = source.EditRowDisplayData.CourtCapacity,
            VenueName = source.EditRowDisplayData.VenueName,
            City = source.EditRowDisplayData.City,
            Date = source.EditRowDisplayData.Date,
            StartsAt = source.EditRowDisplayData.StartsAt,
            ClosesAt = source.EditRowDisplayData.ClosesAt,
            JoinedCount = source.EditRowDisplayData.JoinedCount
        };
    }

    internal static MatchEditResponseModel MatchEditRowToResponseModel(EditRow source)
    {
        var result = new MatchEditResponseModel
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            RowVersion = source.RowVersion,

            EditDisplayDataResponseModel = MatchEditRowToDisplayDataResponseModel(source),
        };

        return result;
    }

    internal static Expression<Func<Court, CourtOptionResponseModel>> MatchCourtOptionProjection()
    {
        return source => new CourtOptionResponseModel
        {
            CourtId = source.Id,
            Name = source.Name,
            Surface = source.Surface,
            Capacity = source.Capacity,
            SportIds = source.Sports.Select(x => x.SportId).ToList()
        };
    }

    internal static Expression<Func<Court, CourtOptionResponseModel>> CourtToCourtOptionProjection()
    {
        return source => new CourtOptionResponseModel
        {
            CourtId = source.Id,
            Name = source.Name,
            Surface = source.Surface,
            Capacity = source.Capacity,
            SportIds = source.Sports.Select(x => x.SportId).ToList()
        };
    }

    internal static MatchOptionsResponseModel MatchOptionsToResponseModel(
        IReadOnlyList<SportOptionResponseModel> sportOptions,
        IReadOnlyList<CourtOptionResponseModel> courtOptions,
        IReadOnlyList<VenueOptionResponseModel> venueOptions)
    {
        return new MatchOptionsResponseModel
        {
            SportOptions = sportOptions,
            CourtOptions = courtOptions,
            VenueOptions = venueOptions
        };
    }


    internal static MatchResponseModel ToMatchResponseModel(
        PageResult<MatchCardResponseModel> page,
        MatchOptionsResponseModel options)
    {
        return new MatchResponseModel
        {
            Page = page,
            Options = options
        };
    }

    internal static Expression<Func<Venue, VenueOptionResponseModel>> VenueToVenueOptionProjection()
    {
        return source => new VenueOptionResponseModel
        {
            Id = source.Id,
            Name = source.Name,
            City = source.City,
            Date = source.Date,
            StartsAt = source.StartsAt,
            ClosesAt = source.ClosesAt
        };
    }

    internal static Expression<Func<Venue, SlotRow>> SlotRowProjection()
    {
        return source => new SlotRow
        {
            Id = source.Id,
            Date = source.Date,
            StartsAt = source.StartsAt,
            OpensAt = source.OpensAt,
            ClosesAt = source.ClosesAt,
            IsTaken = source.Event != null,
            IsDeleted = source.IsDeleted
        };
    }

    internal static Expression<Func<Sport, SportOptionResponseModel>> SportToSportOptionProjection()
    {
        return source => new SportOptionResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }

    internal static Match MatchCreateToEntity(
        MatchCreateRequestData source,
        DateTime nowUtc)
    {
        var result = new Match
        {
            OrganiserId = source.OrganiserId,
            Title = source.Title.Trim(),
            Description = source.Description,
            DurationMinutes = source.DurationMinutes,
            MinPlayers = source.MinPlayers,
            MaxPlayers = source.MaxPlayers,
            MinSkill = source.MinSkill,
            MaxSkill = source.MaxSkill,
            PricePerPlayer = source.PricePerPlayer,
            Status = MatchStatus.Open,
            IsDeleted = false,
            Event = new Event
            {
                SportId = source.SportId,
                CourtId = source.CourtId,
                VenueId = source.VenueId
            }
        };

        result.Participants.Add(new MatchParticipant
        {
            UserId = source.OrganiserId,
            Status = ParticipantStatus.Joined,
            JoinedOn = nowUtc,
            StatusChangedOn = nowUtc,
            IsLateCancel = false
        });

        return result;
    }

    internal static void MatchUpdateApplyToEntity(
        Match target,
        MatchUpdateRequestData source)
    {
        target.Title = source.Title.Trim();
        target.Description = source.Description;
        target.DurationMinutes = source.DurationMinutes;
        target.MinPlayers = source.MinPlayers;
        target.MaxPlayers = source.MaxPlayers;
        target.MinSkill = source.MinSkill;
        target.MaxSkill = source.MaxSkill;
        target.PricePerPlayer = source.PricePerPlayer;
    }

    internal static void MatchCancelApplyToEntity(
        Match target,
        MatchCancelRequestData source)
    {
        target.Status = MatchStatus.Cancelled;
        target.CancelReason = source.Reason.Trim();
    }
}
