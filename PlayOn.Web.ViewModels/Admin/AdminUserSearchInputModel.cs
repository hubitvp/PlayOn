using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminUserSearchInputModel
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        [EnumDataType(typeof(UserStatusFilter))]
        public UserStatusFilter Status { get; set; } = UserStatusFilter.All;

        public int Page { get; set; } = 1;
    }
}
