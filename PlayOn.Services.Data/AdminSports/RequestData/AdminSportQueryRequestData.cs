using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    public class AdminSportQueryRequestData
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = AdminPageSize;
    }
}
