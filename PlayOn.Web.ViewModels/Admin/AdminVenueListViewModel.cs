using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// View model of the administrator venues list: a page of venues and the applied filters.
    /// </summary>
    public class AdminVenueListViewModel : PageViewModel
    {
        /// <summary>
        /// Venues on the current page.
        /// </summary>
        public required IReadOnlyList<AdminVenueItemViewModel> Venues { get; init; } = [];
        /// <summary>
        /// Search term applied to the list, if any.
        /// </summary>
        public string? Search { get; init; }
        /// <summary>
        /// Venue status filter applied to the list.
        /// </summary>
        public VenueStatusFilter Status { get; init; }
        /// <summary>
        /// Total number of items matching the current search.
        /// </summary>
        public required int TotalCount { get; init; }
    }
}
