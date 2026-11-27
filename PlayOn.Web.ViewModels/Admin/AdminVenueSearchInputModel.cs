using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// Input model binding the search, filter and paging parameters of the administrator venues list.
    /// </summary>
    public class AdminVenueSearchInputModel
    {
        /// <summary>
        /// Optional search term used to filter the venues.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        /// <summary>
        /// Filter by venue status.
        /// </summary>
        [EnumDataType(typeof(VenueStatusFilter))]
        public VenueStatusFilter Status { get; set; } = VenueStatusFilter.All;

        /// <summary>
        /// Number of the requested page, starting from 1.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}