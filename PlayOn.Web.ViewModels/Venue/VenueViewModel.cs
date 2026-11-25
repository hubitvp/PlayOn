namespace PlayOn.Web.ViewModels.Venue
{
    public class VenueViewModelData
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string Date { get; init; }
        public required string StartsAt { get; init; }
        public required string OpensAt { get; init; }
        public required string ClosesAt { get; init; }
        public VenueCourtViewModel? Court { get; init; }
        public Guid? MatchId { get; init; }
        public string? MatchTitle { get; init; }
        public string? Slug { get; init; }
    }

    public class VenueViewModel : PageViewModel
    {
        public required IReadOnlyList<VenueViewModelData> Venues { get; init; }
    }
}