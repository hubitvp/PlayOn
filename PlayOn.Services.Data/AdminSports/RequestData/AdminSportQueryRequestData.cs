using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    /// <summary>
    /// Request data for searching and paging through sports in the administration area.
    /// </summary>
    public class AdminSportQueryRequestData
    {
        /// <summary>
        /// Optional search term used to filter the sports.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

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
