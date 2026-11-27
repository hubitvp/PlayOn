using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{

    /// <summary>
    /// A participant listed on the match details page.
    /// </summary>
    public class MatchParticipantResponseModel
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

    /// <summary>
    /// Response model holding the full details of a match, including permissions of the current user.
    /// </summary>
    public class MatchDetailsResponseModel
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public required Guid Id { get; init; }
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
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }
        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        public required int MinPlayers { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        public required SkillLevel MinSkill { get; init; }
        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        public required SkillLevel MaxSkill { get; init; }
        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        public required decimal PricePerPlayer { get; init; }

        /// <summary>
        /// Display name of the user who organises the match.
        /// </summary>
        public required string OrganiserName { get; init; }
        /// <summary>
        /// Reliability score of the organiser, if one has been calculated.
        /// </summary>
        public required int? OrganiserReliability { get; init; }

        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required SportIcon SportIcon { get; init; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string CourtName { get; init; }
        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        public required Surface Surface { get; init; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int CourtCapacity { get; init; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// Street address of the venue.
        /// </summary>
        public required string Address { get; init; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Date on which the match takes place.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public required TimeOnly ClosesAt { get; init; }

        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int Joined { get; init; }
        /// <summary>
        /// Number of players on the waiting list.
        /// </summary>
        public required int Waitlisted { get; init; }


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
        public required IReadOnlyList<MatchParticipantResponseModel> Participants { get; init; }
    }

}
