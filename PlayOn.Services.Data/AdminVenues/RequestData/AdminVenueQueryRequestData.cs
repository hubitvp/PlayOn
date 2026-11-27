using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    /// <summary>
    /// Request data for searching, filtering and paging through venues in the administration area.
    /// </summary>
    public class AdminVenueQueryRequestData
    {
        /// <summary>
        /// Optional search term used to filter the venues.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

        /// <summary>
        /// Filter by booking/availability status.
        /// </summary>
        [EnumDataType(typeof(VenueStatusFilter))]
        public VenueStatusFilter Status { get; set; } = VenueStatusFilter.All;

        /// <summary>
        /// Number of the requested page.
        /// </summary>
        public int Page { get; set; } = FirstPage;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = AdminPageSize;
    }
}
