using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Web.ViewModels.Venue
{
    public class VenueViewModelData
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly OpensAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }
        public required CourtViewModel Court { get; init; }
        public required int MatchId { get; init; }
        public required string MatchTitle { get; init; }
        public required string MatchSlug { get; init; }
    }
    public class VenueViewModel : PagedViewModel
    {
        public required IEnumerable<VenueViewModelData> Venues { get; init; }
    }
}
