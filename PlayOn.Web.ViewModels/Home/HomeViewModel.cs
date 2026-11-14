using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Web.ViewModels.Match;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel : PageViewModel
    {
        public required Guid? SportId { get; init; }
        public required string? City { get; init; }
        public required string? From { get; init; }

        public required int TotalItems { get; init; }

        public required IReadOnlyCollection<HomeMatchCardViewModel> MatchCards { get; init; }

        public required IEnumerable<SelectListItem> Sports { get; init; }
    }

}
