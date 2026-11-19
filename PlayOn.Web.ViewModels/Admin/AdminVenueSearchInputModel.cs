using PlayOn.Common.ApplicationConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminVenueSearchInputModel
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        [EnumDataType(typeof(VenueStatusFilter))]
        public VenueStatusFilter Status { get; set; } = VenueStatusFilter.All;

        public int Page { get; set; } = 1;
    }
}
