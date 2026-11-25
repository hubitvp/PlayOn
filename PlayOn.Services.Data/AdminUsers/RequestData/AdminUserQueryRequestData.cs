using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminUsers.RequestData
{
    public class AdminUserQueryRequestData
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

        [EnumDataType(typeof(UserStatusFilter))]
        public UserStatusFilter Status { get; set; } = UserStatusFilter.All;

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = AdminPageSize;
    }
}
