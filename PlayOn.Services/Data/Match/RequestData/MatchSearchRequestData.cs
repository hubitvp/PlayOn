using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.Matches.RequestData
{
    public class MatchSearchRequestData
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? SearchTerm { get; set; }

        public int? SportId { get; set; }

        [StringLength(MatchRules.CityMaxLength)]
        public string? City { get; set; }

        public DateOnly? From { get; set; }

        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel? Skill { get; set; }

        public bool OnlyWithFreeSpots { get; set; }

        [EnumDataType(typeof(MatchSort))]
        public MatchSort Sort { get; set; } = MatchSort.Soonest;

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = DefaultPageSize;
    }
}
