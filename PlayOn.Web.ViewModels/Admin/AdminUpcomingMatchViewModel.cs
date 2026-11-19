namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminUpcomingMatchViewModel
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required string StartsAt { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required string Status { get; init; }
    }
}
