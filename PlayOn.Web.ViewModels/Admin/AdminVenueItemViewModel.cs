namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminVenueItemViewModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string Date { get; init; }
        public required string StartsAt { get; init; }
        public required string OpeningHours { get; init; }
        public required bool IsDeleted { get; init; }
        public required bool IsBooked { get; init; }
        public string? MatchTitle { get; init; }
    }
}
