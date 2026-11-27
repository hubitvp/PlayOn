using PlayOn.Common.ApplicationConstants;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// Input model binding the search and paging parameters of the administrator sports list.
    /// </summary>
    public class AdminSportSearchInputModel
    {
        /// <summary>
        /// Optional search term used to filter the sports.
        /// </summary>
        [StringLength(MatchRules.SearchTermMaxLength)]
        [Display(Name = "Search")]
        public string? Search { get; set; }

        /// <summary>
        /// Number of the requested page, starting from 1.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}