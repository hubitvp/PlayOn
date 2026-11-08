using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchDetailsViewModel
    {
        public required int Id { get; init; }
        public required string Slug { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }
        public required MatchStatus Status { get; init; }
        public string? CancelReason { get; init; }
        public required bool IsHidden { get; init; }

        public required string SportName { get; init; }
        public required string SportIcon { get; init; }
        public required string StartsAt { get; init; }
        public required string EndsAt { get; init; }
        public required int DurationMinutes { get; init; }

        public required string VenueName { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string CourtName { get; init; }
        public required Surface Surface { get; init; }

        public required int MinPlayers { get; init; }
        public required int MaxPlayers { get; init; }
        public required int Joined { get; init; }
        public required int Waitlisted { get; init; }
        public required SkillLevel MinSkill { get; init; }
        public required SkillLevel MaxSkill { get; init; }
        public required string PricePerPlayer { get; init; }

        public required string OrganiserName { get; init; }
        public int? OrganiserReliability { get; init; }

        public required bool CanManage { get; init; }

        public required IReadOnlyList<MatchParticipantViewModel> Participants { get; init; }
    }

    public class MatchParticipantViewModel
    {
        public required string DisplayName { get; init; }
        public required string City { get; init; }
        public required ParticipantStatus Status { get; init; }
        public required bool IsOrganiser { get; init; }
        public int? Reliability { get; init; }
    }
}
