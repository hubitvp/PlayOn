using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminSportSearchInputModel
    {
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}