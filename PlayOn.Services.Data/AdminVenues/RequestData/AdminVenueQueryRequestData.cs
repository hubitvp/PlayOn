using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    public class AdminVenueQueryRequestData
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        public string? Search { get; set; }

        [EnumDataType(typeof(VenueStatusFilter))]
        public VenueStatusFilter Status { get; set; } = VenueStatusFilter.All;

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = AdminPageSize;
    }
}
