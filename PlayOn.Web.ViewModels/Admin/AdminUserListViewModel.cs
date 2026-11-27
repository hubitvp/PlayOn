using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// View model of the administrator users list: a page of users and the applied filters.
    /// </summary>
    public class AdminUserListViewModel : PageViewModel
    {
        /// <summary>
        /// Users on the current page.
        /// </summary>
        public required IReadOnlyList<AdminUserItemViewModel> Users { get; init; } = [];
        /// <summary>
        /// Search term applied to the list, if any.
        /// </summary>
        public string? Search { get; init; }
        /// <summary>
        /// Account status filter applied to the list.
        /// </summary>
        public UserStatusFilter Status { get; init; }
        /// <summary>
        /// Total number of items matching the current search.
        /// </summary>
        public required int TotalCount { get; init; }
    }
}
