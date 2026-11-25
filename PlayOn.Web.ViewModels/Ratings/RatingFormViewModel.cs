using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.PlayerRatingConstants;

namespace PlayOn.Web.ViewModels.Ratings
{
    public class RatingFormViewModel
    {
        [NotEmptyGuid(ErrorMessage = "The match is not valid.")]
        public Guid MatchId { get; set; }

        [ValidateNever]
        public string MatchTitle { get; set; } = null!;

        public List<RatingPlayerInputModel> Players { get; set; } = [];
    }

    public class RatingPlayerInputModel
    {
        [NotEmptyGuid(ErrorMessage = "The player is not valid.")]
        public Guid UserId { get; set; }

        [ValidateNever]
        public string DisplayName { get; set; } = null!;

        [ValidateNever]
        public string City { get; set; } = null!;

        [Range(MinRatingScore, MaxRatingScore, ErrorMessage = "The score must be between {1} and {2}.")]
        public int? Score { get; set; }

        [StringLength(CommentMaxLength)]
        public string? Comment { get; set; }
    }
}