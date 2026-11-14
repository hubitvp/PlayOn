using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.ParticipationRules;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;

namespace PlayOn.Web.ViewModels.Ratings
{
    public class RatingFormViewModel
    {
        public Guid MatchId { get; set; }

        [ValidateNever]
        public string MatchTitle { get; set; } = string.Empty;

        public List<RatingPlayerInputModel> Players { get; set; } = [];
    }

    public class RatingPlayerInputModel
    {
        [Required]
        public Guid UserId { get; set; }

        [ValidateNever]
        public string DisplayName { get; set; } = string.Empty;

        [ValidateNever]
        public string City { get; set; } = string.Empty;

        [Range(MinRatingScore, MaxRatingScore, ErrorMessage = "The score must be between 1 and 5.")]
        public int? Score { get; set; }

        [StringLength(CommentMaxLength)]
        public string? Comment { get; set; }
    }
}
