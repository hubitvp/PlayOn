using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    /// <summary>
    /// Request data for searching, filtering and paging through users in the administration area.
    /// </summary>
    public class AdminUserQueryRequestData
    {
        /// <summary>
        /// Optional search term used to filter the users.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

        /// <summary>
        /// Filter by account status.
        /// </summary>
        [EnumDataType(typeof(UserStatusFilter))]
        public UserStatusFilter Status { get; set; } = UserStatusFilter.All;

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
