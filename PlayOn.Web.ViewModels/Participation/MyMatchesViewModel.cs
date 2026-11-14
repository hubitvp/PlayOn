using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Participation
{
    public class MyMatchesViewModel : PageViewModel
    {
        public required MyMatchesTab Tab { get; init; }

        public required IReadOnlyList<MyMatchViewModel> Matches { get; init; }
    }

    public class MyMatchViewModel
    {
        public required Guid Id { get; init; }
        public required string Slug { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required string SportIcon { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required string StartsAt { get; init; }
        public required ParticipantStatus Status { get; init; }
        public required MatchStatus MatchStatus { get; init; }
        public required int WaitlistPosition { get; init; }
        public required bool CanLeave { get; init; }
        public required bool CanRate { get; init; }
    }
}
