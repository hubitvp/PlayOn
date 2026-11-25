using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminUserListViewModel : PageViewModel
    {
        public required IReadOnlyList<AdminUserItemViewModel> Users { get; init; } = [];
        public string? Search { get; init; }
        public UserStatusFilter Status { get; init; }
        public required int TotalCount { get; init; }
    }
}
