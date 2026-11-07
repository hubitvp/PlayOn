using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Web.ViewModels.Match;


namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel : PagedViewModel
    {
        public int? SportId { get; init; }
        public string? City { get; init; }
        public string? From { get; init; }

        public int TotalItems { get; init; }
        public required IReadOnlyCollection<MatchCardViewModel> MatchCards { get; init; }
        public required IEnumerable<SelectListItem> Sports { get; init; }
    }

}
