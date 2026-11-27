namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// View model of the administrator sports list: a page of sports and the applied search.
    /// </summary>
    public class AdminSportListViewModel : PageViewModel
    {
        /// <summary>
        /// Sports on the current page.
        /// </summary>
        public required IReadOnlyList<AdminSportItemViewModel> Sports { get; init; } = [];
        /// <summary>
        /// Search term applied to the list, if any.
        /// </summary>
        public string? Search { get; init; }
        /// <summary>
        /// Total number of items matching the current search.
        /// </summary>
        public required int TotalCount { get; init; }
    }
}
