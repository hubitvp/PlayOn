using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Participation.ResponseModels
{
    /// <summary>
    /// Response model describing the result of joining a match.
    /// </summary>
    public class JoinMatchResponseModel
    {
        /// <summary>
        /// Resulting participation status of the user (e.g. joined or waitlisted).
        /// </summary>
        public required ParticipantStatus Status { get; init; }
        /// <summary>
        /// Position on the waiting list, or 0 if the user is not on it.
        /// </summary>
        public required int WaitlistPosition { get; init; }
        /// <summary>
        /// Indicates whether the match is confirmed after the user joined.
        /// </summary>
        public required bool MatchConfirmed { get; init; }
    }

    /// <summary>
    /// Response model describing the result of leaving a match.
    /// </summary>
    public class LeaveMatchResponseModel
    {
        /// <summary>
        /// Indicates whether the user left too close to the start of the match.
        /// </summary>
        public required bool IsLateCancel { get; init; }
        /// <summary>
        /// Indicates whether a player from the waiting list was promoted to take the free spot.
        /// </summary>
        public required bool PromotedAnotherPlayer { get; init; }
    }

    /// <summary>
    /// A match in the list of matches of the current user.
    /// </summary>
    public class MyMatchResponseModel
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
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required SportIcon SportIcon { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// City in which the venue is located.
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
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }
        /// <summary>
        /// Participation status of the user in the match.
        /// </summary>
        public required ParticipantStatus Status { get; init; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public required MatchStatus MatchStatus { get; init; }
        /// <summary>
        /// Position on the waiting list, or 0 if the user is not on it.
        /// </summary>
        public required int WaitlistPosition { get; init; }
        /// <summary>
        /// Indicates whether the user is the organiser of the match.
        /// </summary>
        public required bool IsOrganiser { get; init; }
        /// <summary>
        /// Indicates whether the current user can rate the players of the match.
        /// </summary>
        public required bool CanRate { get; init; }
    }
}
