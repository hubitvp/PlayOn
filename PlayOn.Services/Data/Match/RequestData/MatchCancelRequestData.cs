using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchCancelRequestData : IValidatableObject
    {
        [Required]
        public Guid MatchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool UserIsAdmin { get; set; }

        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = CancelReasonMinLength)]
        public string Reason { get; set; } = null!;

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
