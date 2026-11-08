using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches;
using PlayOn.Services.Data.Matches.ResponseModels;
using PlayOn.Services.Data.Venue.ResponseModels;
using Riok.Mapperly.Abstractions;

namespace PlayOn.Services.Mappers;

[Mapper]
public partial class ServiceMapper
{
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.SportName), Use = nameof(MapSportName))]
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.SportIcon), Use = nameof(MapSportIcon))]
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.VenueName), Use = nameof(MapVenueName))]
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.City), Use = nameof(MapCity))]
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.Date), Use = nameof(MapDate))]
    [MapProperty(nameof(Match.Event), nameof(MatchCardResponseModel.StartsAt), Use = nameof(MapStartsAt))]
    [MapProperty(nameof(Match.Participants), nameof(MatchCardResponseModel.Joined), Use = nameof(MapJoined))]
    [MapProperty(nameof(Match.Participants), nameof(MatchCardResponseModel.Waitlisted), Use = nameof(MapWaitlisted))]
    [MapperIgnoreSource(nameof(Match.OrganiserId))]
    [MapperIgnoreSource(nameof(Match.Organiser))]
    [MapperIgnoreSource(nameof(Match.Description))]
    [MapperIgnoreSource(nameof(Match.MinPlayers))]
    [MapperIgnoreSource(nameof(Match.MaxSkill))]
    [MapperIgnoreSource(nameof(Match.CancelReason))]
    [MapperIgnoreSource(nameof(Match.IsHidden))]
    [MapperIgnoreSource(nameof(Match.IsDeleted))]
    [MapperIgnoreSource(nameof(Match.RowVersion))]
    [MapperIgnoreSource(nameof(Match.Ratings))]
    public partial MatchCardResponseModel ToMatchCardResponse(Match source);

    public MatchDetailsResponseModel ToMatchDetailsResponse(
        DetailsRow source,
        bool canManage)
        => new()
        {
            Id = source.Id,
            Title = source.Title,
            Description = source.Description,
            Status = source.Status,
            CancelReason = source.CancelReason,
            IsHidden = source.IsHidden,
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
            Joined = source.Participants.Count(x => x.Status == ParticipantStatus.Joined),
            Waitlisted = source.Participants.Count(x => x.Status == ParticipantStatus.Waitlisted),
            CanManage = canManage,
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


    [MapperIgnoreSource(nameof(Sport.Icon))]
    [MapperIgnoreSource(nameof(Sport.DefaultMinPlayers))]
    [MapperIgnoreSource(nameof(Sport.DefaultMaxPlayers))]
    [MapperIgnoreSource(nameof(Sport.Courts))]
    [MapperIgnoreSource(nameof(Sport.Preferences))]
    [MapperIgnoreSource(nameof(Sport.Events))]
    public partial SportsResponseModel ToSportsResponse(Sport source);

    [MapProperty(nameof(Venue.Event), nameof(VenueResponseModel.Court), Use = nameof(MapCourt))]
    [MapProperty(nameof(Venue.Event), nameof(VenueResponseModel.MatchId), Use = nameof(MapMatchId))]
    [MapProperty(nameof(Venue.Event), nameof(VenueResponseModel.MatchTitle), Use = nameof(MapMatchTitle))]
    [MapperIgnoreSource(nameof(Venue.Latitude))]
    [MapperIgnoreSource(nameof(Venue.Longitude))]
    [MapperIgnoreSource(nameof(Venue.PhotoUrl))]
    [MapperIgnoreSource(nameof(Venue.IsDeleted))]
    public partial VenueResponseModel ToVenueResponse(Venue source);

    private static string MapSportName(Event source) => source.Sport.Name;

    private static SportIcon MapSportIcon(Event source) => source.Sport.Icon;

    private static string MapVenueName(Event source) => source.Venue.Name;

    private static string MapCity(Event source) => source.Venue.City;

    private static DateOnly MapDate(Event source) => source.Venue.Date;

    private static TimeOnly MapStartsAt(Event source) => source.Venue.StartsAt;

    private static int MapJoined(ICollection<MatchParticipant> source)
        => source.Count(x => x.Status == ParticipantStatus.Joined);

    private static int MapWaitlisted(ICollection<MatchParticipant> source)
        => source.Count(x => x.Status == ParticipantStatus.Waitlisted);

    private static CourtResponseModel MapCourt(Event source)
        => new()
        {
            Name = source.Court.Name,
            SportIcon = source.Sport.Icon,
            Surface = source.Court.Surface,
            Capacity = source.Court.Capacity,
            HourlyPrice = source.Court.HourlyPrice
        };

    private static int MapMatchId(Event source) => source.MatchId;

    private static string MapMatchTitle(Event source) => source.Match.Title;
}
