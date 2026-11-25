using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeMatchCardViewModel
    {
        public required Guid Id { get; init; }
        public required string Slug { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required string SportIcon { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required string StartsAt { get; init; }
        public required int DurationMinutes { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required int Waitlisted { get; init; }
        public required SkillLevel? MinSkill { get; init; }
        public required string PricePerPlayer { get; init; }
        public required MatchStatus Status { get; init; }
    }
}