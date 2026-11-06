using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchCardViewModel
    {

        public required int Id { get; init; }
        public string Slug { get; set; } = null!;
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public string SportIcon { get; set; } = null!;
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public string StartsAt { get; set; } = null!;
        public required int DurationMinutes { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required int Waitlisted { get; init; }
        public required SkillLevel? MinSkill { get; init; }
        public string PricePerPlayer { get; set; } = null!;
        public required MatchStatus Status { get; init; }
    }
}
