using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Common.Enums;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchListViewModel : PageViewModel
    {
        public required string? SearchTerm { get; init; }
        public required Guid? SportId { get; init; }
        public required string? City { get; init; }
        public required string? From { get; init; }
        public required SkillLevel? Skill { get; init; }
        public required bool OnlyWithFreeSpots { get; init; }
        public required MatchSort Sort { get; init; }

        public required int TotalItems { get; init; }

        public required IEnumerable<SelectListItem> Sports { get; init; }
        public required IReadOnlyCollection<MatchCardViewModel> Matches { get; init; }
    }
}
