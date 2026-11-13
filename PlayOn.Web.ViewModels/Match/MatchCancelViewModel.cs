using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchCancelViewModel : IValidatableObject
    {

        public Guid MatchId { get; set; }

        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = CancelReasonMinLength)]
        public string Reason { get; set; } = null!;


        public string Title { get; set; } = null!;
        public string StartsAt { get; set; } = null!;
        public string VenueName { get; set; } = null!;
        public int JoinedCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Reason.Trim().Length < CancelReasonMinLength)
            {
                yield return new ValidationResult(
                    $"Give a reason of at least {CancelReasonMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Reason) });
            }
        }
    }
}
