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
        public required VenueCourtViewModel Court { get; init; }
        public required Guid MatchId { get; init; }
        public required string MatchTitle { get; init; }
        public string MatchSlug { get; set; } = null!;
    }
    public class VenueViewModel : PageViewModel
    {
        public required IEnumerable<VenueViewModelData> Venues { get; init; }
    }
}
