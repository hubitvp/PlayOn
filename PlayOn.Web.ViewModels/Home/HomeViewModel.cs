using Microsoft.AspNetCore.Mvc.Rendering;


namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel : PageViewModel
    {
        public Guid? SportId { get; init; }
        public string? City { get; init; }
        public string? From { get; init; }

        public int TotalItems { get; init; }

        public required IReadOnlyCollection<HomeMatchCardViewModel> MatchCards { get; init; }

        public required IEnumerable<SelectListItem> Sports { get; init; }
    }

}
