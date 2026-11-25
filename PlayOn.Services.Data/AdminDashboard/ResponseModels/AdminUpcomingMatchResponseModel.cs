using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminDashboard.ResponseModels
{
    public class AdminUpcomingMatchResponseModel
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required MatchStatus Status { get; init; }
    }
}
