using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Participation.ResponseModels
{
    public class JoinMatchResponseModel
    {
        public required ParticipantStatus Status { get; init; }
        public required int WaitlistPosition { get; init; }
        public required bool MatchConfirmed { get; init; }
    }

    public class LeaveMatchResponseModel
    {
        public required bool IsLateCancel { get; init; }
        public required bool PromotedAnotherPlayer { get; init; }
    }

    public class MyMatchResponseModel
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required int DurationMinutes { get; init; }
        public required ParticipantStatus Status { get; init; }
        public required MatchStatus MatchStatus { get; init; }
        public required int WaitlistPosition { get; init; }
        public required bool IsOrganiser { get; init; }
        public required bool CanRate { get; init; }
    }
}
