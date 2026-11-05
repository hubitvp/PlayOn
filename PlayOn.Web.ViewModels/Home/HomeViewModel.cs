using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Web.ViewModels.Match;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public int? SportId { get; init; }
        public string? City { get; init; }
        public string? From { get; init; }

        public required IReadOnlyCollection<MatchCardViewModel> MatchCards { get; init; }
        public required IEnumerable<SelectListItem> Sports { get; init; }


        public int Page { get; init; } = FirstPage;
        public int PageSize { get; init; } = DefaultPageSize;
        public int TotalPages { get; init; } = 0;
    }

}
