using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// View model of the match details page, including what the current user is allowed to do.
    /// </summary>
    public class MatchDetailsViewModel
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// URL-friendly slug of the match, used to build its address.
        /// </summary>
        public required string Slug { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Optional description of the match.
        /// </summary>
        public required string? Description { get; init; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public required MatchStatus Status { get; init; }
        /// <summary>
        /// Reason given when the match was cancelled, if any.
        /// </summary>
        public required string? CancelReason { get; init; }

        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required string SportIcon { get; init; }
        /// <summary>
        /// Formatted date and time the match starts.
        /// </summary>
        public required string StartsAt { get; init; }
        /// <summary>
        /// Formatted date and time the match ends.
        /// </summary>
        public required string EndsAt { get; init; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// Street address of the venue.
        /// </summary>
        public required string Address { get; init; }
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string CourtName { get; init; }
        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        public required Surface Surface { get; init; }

        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        public required int MinPlayers { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int Joined { get; init; }
        /// <summary>
        /// Number of players on the waiting list of the match.
        /// </summary>
        public required int Waitlisted { get; init; }
        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        public required SkillLevel MinSkill { get; init; }
        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        public required SkillLevel MaxSkill { get; init; }
        /// <summary>
        /// Formatted price each player pays to join the match.
        /// </summary>
        public required string PricePerPlayer { get; init; }

        /// <summary>
        /// Display name of the user who organises the match.
        /// </summary>
        public required string OrganiserName { get; init; }
        /// <summary>
        /// Reliability score of the organiser, if one has been calculated.
        /// </summary>
        public required int? OrganiserReliability { get; init; }

        /// <summary>
        /// Indicates whether the current user can manage (edit or cancel) the match.
        /// </summary>
        public required bool CanManage { get; init; }

        /// <summary>
        /// Participation status of the current user in the match, or <c>null</c> if the user has not joined.
        /// </summary>
        public ParticipantStatus? CurrentUserStatus { get; init; }
        /// <summary>
        /// Indicates whether the current user can join the match.
        /// </summary>
        public required bool CanJoin { get; init; }
        /// <summary>
        /// Indicates whether the current user can leave the match.
        /// </summary>
        public required bool CanLeave { get; init; }
        /// <summary>
        /// Indicates whether the current user can take attendance for the match.
        /// </summary>
        public required bool CanTakeAttendance { get; init; }
        /// <summary>
        /// Indicates whether the current user can rate the players of the match.
        /// </summary>
        public required bool CanRate { get; init; }

        /// <summary>
        /// Participants of the match.
        /// </summary>
        public required IReadOnlyList<MatchParticipantViewModel> Participants { get; init; }
    }

    /// <summary>
    /// A participant listed on the match details page.
    /// </summary>
    public class MatchParticipantViewModel
    {
        /// <summary>
        /// Identifier of the participant.
        /// </summary>
        public required Guid UserId { get; init; }
        /// <summary>
        /// Display name of the participant.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// City of the participant.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Participation status of the user (e.g. joined or waitlisted).
        /// </summary>
        public required ParticipantStatus Status { get; init; }
        /// <summary>
        /// Indicates whether the user is the organiser of the match.
        /// </summary>
        public required bool IsOrganiser { get; init; }
        /// <summary>
        /// Reliability score of the participant, if one has been calculated.
        /// </summary>
        public required int? Reliability { get; init; }
    }
}
