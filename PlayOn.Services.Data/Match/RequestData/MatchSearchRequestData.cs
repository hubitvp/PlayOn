using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    /// <summary>
    /// Request data for searching and filtering matches.
    /// </summary>
    public class MatchSearchRequestData
    {
        /// <summary>
        /// Optional search term used to filter the matches.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Optional sport used to filter the matches.
        /// </summary>
        public Guid? SportId { get; set; }

        /// <summary>
        /// Optional city used to filter the matches.
        /// </summary>
        [StringLength(MatchRules.CityMaxLength)]
        public string? City { get; set; }

        /// <summary>
        /// Optional date from which matches are returned.
        /// </summary>
        public DateOnly? From { get; set; }

        /// <summary>
        /// Optional skill level used to filter the matches.
        /// </summary>
        [EnumDataType(typeof(SkillLevel))]
        public SkillLevel? Skill { get; set; }

        /// <summary>
        /// When <c>true</c>, only matches that still have free spots are returned.
        /// </summary>
        public bool OnlyWithFreeSpots { get; set; }

        /// <summary>
        /// Order in which the matches are returned.
        /// </summary>
        [EnumDataType(typeof(MatchSort))]
        public MatchSort Sort { get; set; } = MatchSort.Soonest;

    }
}
