using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// Input model binding the search, filter and paging parameters of the administrator users list.
    /// </summary>
    public class AdminUserSearchInputModel
    {
        /// <summary>
        /// Optional search term used to filter the users.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        /// <summary>
        /// Filter by account status.
        /// </summary>
        [EnumDataType(typeof(UserStatusFilter))]
        public UserStatusFilter Status { get; set; } = UserStatusFilter.All;

        /// <summary>
        /// Number of the requested page, starting from 1.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}