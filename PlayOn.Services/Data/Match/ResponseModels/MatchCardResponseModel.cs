using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{
    public class MatchCardResponseModel
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; } = null!;
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required int DurationMinutes { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required int Waitlisted { get; init; }
        public required SkillLevel? MinSkill { get; init; }
        public required decimal PricePerPlayer { get; init; }
        public required MatchStatus Status { get; init; }
    }
}
