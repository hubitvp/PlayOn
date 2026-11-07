using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchListViewModel : PagedViewModel
    {
        public string? SearchTerm { get; init; }
        public int? SportId { get; init; }
        public string? City { get; init; }
        public string? From { get; init; }
        public SkillLevel? Skill { get; init; }
        public bool OnlyWithFreeSpots { get; init; }
        public MatchSort Sort { get; init; }

        public required int TotalItems { get; init; }

        public IReadOnlyList<SelectListItem> Sports { get; init; } = [];
        public IReadOnlyList<MatchCardViewModel> Matches { get; init; } = [];
    }
}
