using PlayOn.Common.Enums;
namespace PlayOn.Data.Seeding
{
    /// <summary>
    /// Seed data for a user account, including the role to assign and an optional password.
    /// </summary>
    /// <param name="UserName">User name of the account.</param>
    /// <param name="Email">Email address of the user.</param>
    /// <param name="FullName">Full name of the user.</param>
    /// <param name="DisplayName">Display name of the user.</param>
    /// <param name="Address">Home address of the user.</param>
    /// <param name="JoinDate">Date the user joined.</param>
    /// <param name="City">City of the user.</param>
    /// <param name="IsActive">Indicates whether the account is active.</param>
    /// <param name="ReliabilityScore">Reliability score of the user, if any.</param>
    /// <param name="MatchesPlayed">Number of matches the user has played.</param>
    /// <param name="Role">Name of the role to assign to the user.</param>
    /// <param name="Password">Optional password used when the user is created; a user that does not exist yet and has no password is skipped.</param>
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

    /// <summary>
    /// Seed data for a court.
    /// </summary>
    /// <param name="Name">Name of the court; used to reference the court from other seed data.</param>
    /// <param name="Surface">Type of playing surface of the court.</param>
    /// <param name="Capacity">Maximum number of people the court can accommodate.</param>
    /// <param name="HourlyPrice">Price for renting the court for one hour.</param>
    public sealed record SeedCourtDto(
        string Name,
        Surface Surface,
        int Capacity,
        decimal HourlyPrice);

    /// <summary>
    /// Seed data linking a court to a sport that can be played on it. Court and sport are referenced by name.
    /// </summary>
    /// <param name="CourtName">Name of the court.</param>
    /// <param name="SportName">Name of the sport.</param>
    public sealed record SeedCourtSportDto(
        string CourtName,
        string SportName);

    /// <summary>
    /// Seed data for a sport.
    /// </summary>
    /// <param name="Name">Name of the sport; used to reference the sport from other seed data.</param>
    /// <param name="Icon">Icon representing the sport.</param>
    /// <param name="DefaultMinPlayers">Default minimum number of players for matches of the sport.</param>
    /// <param name="DefaultMaxPlayers">Default maximum number of players for matches of the sport.</param>
    public sealed record SeedSportDto(
        string Name,
        SportIcon Icon,
        int DefaultMinPlayers,
        int DefaultMaxPlayers);

    /// <summary>
    /// Seed data for a venue and its bookable slot.
    /// </summary>
    /// <param name="Date">Date of the venue slot.</param>
    /// <param name="StartsAt">Time the slot starts.</param>
    /// <param name="Name">Name of the venue.</param>
    /// <param name="Address">Street address of the venue.</param>
    /// <param name="City">City in which the venue is located.</param>
    /// <param name="Latitude">Geographic latitude of the venue.</param>
    /// <param name="Longitude">Geographic longitude of the venue.</param>
    /// <param name="OpensAt">Time the venue opens.</param>
    /// <param name="ClosesAt">Time the venue closes.</param>
    /// <param name="IsDeleted">Soft-delete flag of the venue.</param>
    public sealed record SeedVenueDto(
        DateOnly Date,
        TimeOnly StartsAt,
        string Name,
        string Address,
        string City,
        decimal Latitude,
        decimal Longitude,
        TimeOnly OpensAt,
        TimeOnly ClosesAt,
        bool IsDeleted);

    /// <summary>
    /// Seed data for a user's preferred sport and skill level. User and sport are referenced by email and name.
    /// </summary>
    /// <param name="UserEmail">Email address of the user.</param>
    /// <param name="SportName">Name of the sport.</param>
    /// <param name="SkillLevel">Skill level of the user in the sport.</param>
    public sealed record SeedSportPreferenceDto(
        string UserEmail,
        string SportName,
        SkillLevel SkillLevel);

    /// <summary>
    /// Seed data for a match. The organiser is referenced by email.
    /// </summary>
    /// <param name="Title">Title of the match; used to reference the match from other seed data.</param>
    /// <param name="OrganiserEmail">Email address of the user who organises the match.</param>
    /// <param name="Description">Description of the match.</param>
    /// <param name="DurationMinutes">Duration of the match in minutes.</param>
    /// <param name="MinPlayers">Minimum number of players.</param>
    /// <param name="MaxPlayers">Maximum number of players.</param>
    /// <param name="MinSkill">Minimum skill level required.</param>
    /// <param name="MaxSkill">Maximum skill level allowed.</param>
    /// <param name="PricePerPlayer">Price each player pays to join the match.</param>
    /// <param name="Status">Status of the match.</param>
    /// <param name="CancelReason">Reason the match was cancelled, if it was.</param>
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

    /// <summary>
    /// Seed data linking a match to a sport, a court and a venue. Match, sport and court are referenced by name or title, the venue by its position in the bundle's venue list.
    /// </summary>
    /// <param name="MatchTitle">Title of the match.</param>
    /// <param name="SportName">Name of the sport.</param>
    /// <param name="CourtName">Name of the court.</param>
    /// <param name="VenueIndex">Zero-based position of the venue in the bundle's venue list.</param>
    public sealed record SeedEventDto(
        string MatchTitle,
        string SportName,
        string CourtName,
        int VenueIndex);

    /// <summary>
    /// Seed data for a user's participation in a match. Match and user are referenced by title and email.
    /// </summary>
    /// <param name="MatchTitle">Title of the match.</param>
    /// <param name="UserEmail">Email address of the user.</param>
    /// <param name="Status">Participation status of the user.</param>
    /// <param name="JoinedOn">Date and time the user joined the match.</param>
    /// <param name="StatusChangedOn">Date and time the participation status last changed.</param>
    /// <param name="IsLateCancel">Indicates whether the user cancelled too close to the start of the match.</param>
    public sealed record SeedMatchParticipantDto(
        string MatchTitle,
        string UserEmail,
        ParticipantStatus Status,
        DateTime JoinedOn,
        DateTime StatusChangedOn,
        bool IsLateCancel);

    /// <summary>
    /// Seed data for a notification. User and match are referenced by email and title.
    /// </summary>
    /// <param name="UserEmail">Email address of the user.</param>
    /// <param name="Type">Type of the notification.</param>
    /// <param name="Message">Text of the notification.</param>
    /// <param name="MatchTitle">Title of the match.</param>
    /// <param name="IsRead">Indicates whether the notification has been read.</param>
    /// <param name="CreatedOn">Date and time the notification was created.</param>
    public sealed record SeedNotificationDto(
        string UserEmail,
        NotificationType Type,
        string Message,
        string MatchTitle,
        bool IsRead,
        DateTime CreatedOn);

    /// <summary>
    /// Seed data for a rating one player gives another after a match. Match and players are referenced by title and email.
    /// </summary>
    /// <param name="MatchTitle">Title of the match.</param>
    /// <param name="RaterEmail">Email address of the user giving the rating.</param>
    /// <param name="RatedEmail">Email address of the user being rated.</param>
    /// <param name="Score">Score given to the rated player.</param>
    /// <param name="Comment">Optional comment accompanying the rating.</param>
    public sealed record SeedPlayerRatingDto(
        string MatchTitle,
        string RaterEmail,
        string RatedEmail,
        int Score,
        string? Comment);

    /// <summary>
    /// Container for all seed data. It is serialized to and read from the JSON seed file.
    /// </summary>
    public sealed class SeedDataBundle
    {
        /// <summary>
        /// Names of the roles to create.
        /// </summary>
        public List<string> Roles { get; set; } = [];
        /// <summary>
        /// Users to create, each with a role.
        /// </summary>
        public List<SeedUserDto> Users { get; set; } = [];
        /// <summary>
        /// Courts to create.
        /// </summary>
        public List<SeedCourtDto> Courts { get; set; } = [];
        /// <summary>
        /// Sports to create.
        /// </summary>
        public List<SeedSportDto> Sports { get; set; } = [];
        /// <summary>
        /// Venues (slots) to create. Events refer to venues by their position in this list.
        /// </summary>
        public List<SeedVenueDto> Venues { get; set; } = [];
        /// <summary>
        /// Preferred sports of the users.
        /// </summary>
        public List<SeedSportPreferenceDto> SportPreferences { get; set; } = [];
        /// <summary>
        /// Mappings between courts and the sports that can be played on them.
        /// </summary>
        public List<SeedCourtSportDto> CourtSports { get; set; } = [];
        /// <summary>
        /// Matches to create.
        /// </summary>
        public List<SeedMatchDto> Matches { get; set; } = [];
        /// <summary>
        /// Events linking matches to sports, courts and venues.
        /// </summary>
        public List<SeedEventDto> Events { get; set; } = [];
        /// <summary>
        /// Participation of users in matches.
        /// </summary>
        public List<SeedMatchParticipantDto> MatchParticipants { get; set; } = [];
        /// <summary>
        /// Notifications to create for users.
        /// </summary>
        public List<SeedNotificationDto> Notifications { get; set; } = [];
        /// <summary>
        /// Ratings players give each other after matches.
        /// </summary>
        public List<SeedPlayerRatingDto> PlayerRatings { get; set; } = [];
    }
}