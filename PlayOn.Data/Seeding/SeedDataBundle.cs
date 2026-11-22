using PlayOn.Common.Enums;
namespace PlayOn.Data.Seeding
{
    public sealed record SeedUserDto(
        string UserName,
        string Email,
        string FullName,
        string DisplayName,
        string Address,
        DateOnly JoinDate,
        string City,
        bool IsActive,
        int? ReliabilityScore,
        int MatchesPlayed,
        string Role,
        string? Password = null);

    public sealed record SeedCourtDto(
        string Name,
        Surface Surface,
        int Capacity,
        decimal HourlyPrice);

    public sealed record SeedCourtSportDto(
        string CourtName,
        string SportName);

    public sealed record SeedSportDto(
        string Name,
        SportIcon Icon,
        int DefaultMinPlayers,
        int DefaultMaxPlayers);

    public sealed record SeedVenueDto(
        DateOnly Date,
        TimeOnly StartsAt,
        string Name,
        string Address,
        string City,
        decimal Latitude,
        decimal Longitude,
        string? PhotoUrl,
        TimeOnly OpensAt,
        TimeOnly ClosesAt,
        bool IsDeleted);

    public sealed record SeedSportPreferenceDto(
        string UserEmail,
        string SportName,
        SkillLevel SkillLevel);

    public sealed record SeedMatchDto(
        string Title,
        string OrganiserEmail,
        string Description,
        int DurationMinutes,
        int MinPlayers,
        int MaxPlayers,
        SkillLevel MinSkill,
        SkillLevel MaxSkill,
        decimal PricePerPlayer,
        MatchStatus Status,
        string? CancelReason);

    public sealed record SeedEventDto(
        string MatchTitle,
        string SportName,
        string CourtName,
        int VenueIndex);

    public sealed record SeedMatchParticipantDto(
        string MatchTitle,
        string UserEmail,
        ParticipantStatus Status,
        DateTime JoinedOn,
        DateTime StatusChangedOn,
        bool IsLateCancel);

    public sealed record SeedNotificationDto(
        string UserEmail,
        NotificationType Type,
        string Message,
        string MatchTitle,
        bool IsRead,
        DateTime CreatedOn);

    public sealed record SeedPlayerRatingDto(
        string MatchTitle,
        string RaterEmail,
        string RatedEmail,
        int Score,
        string? Comment);

    public sealed class SeedDataBundle
    {
        public List<string> Roles { get; set; } = [];
        public List<SeedUserDto> Users { get; set; } = [];
        public List<SeedCourtDto> Courts { get; set; } = [];
        public List<SeedSportDto> Sports { get; set; } = [];
        public List<SeedVenueDto> Venues { get; set; } = [];
        public List<SeedSportPreferenceDto> SportPreferences { get; set; } = [];
        public List<SeedCourtSportDto> CourtSports { get; set; } = [];
        public List<SeedMatchDto> Matches { get; set; } = [];
        public List<SeedEventDto> Events { get; set; } = [];
        public List<SeedMatchParticipantDto> MatchParticipants { get; set; } = [];
        public List<SeedNotificationDto> Notifications { get; set; } = [];
        public List<SeedPlayerRatingDto> PlayerRatings { get; set; } = [];
    }
}