namespace PlayOn.Web.ViewModels.Venue
{
    public class VenueViewModelData
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string Date { get; init; }
        public required string StartsAt { get; init; }
        public required string OpensAt { get; init; }
        public required string ClosesAt { get; init; }
        public CourtViewModel Court { get; set; } = null!;
        public required int MatchId { get; init; }
        public required string MatchTitle { get; init; }
        public string MatchSlug { get; set; } = null!;
    }
    public class VenueViewModel : PagedViewModel
    {
        public required IEnumerable<VenueViewModelData> Venues { get; init; }
    }
}
