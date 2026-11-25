namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminSportListViewModel : PageViewModel
    {
        public required IReadOnlyList<AdminSportItemViewModel> Sports { get; init; } = [];
        public string? Search { get; init; }
        public required int TotalCount { get; init; }
    }
}
