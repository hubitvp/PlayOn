using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminVenueListViewModel : PageViewModel
    {
        public required IReadOnlyList<AdminVenueItemViewModel> Venues { get; init; } = [];
        public string? Search { get; init; }
        public VenueStatusFilter Status { get; init; }
        public required int TotalCount { get; init; }
    }
}
