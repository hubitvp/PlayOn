using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchCancelViewModel : IValidatableObject
    {
        public Guid MatchId { get; set; }

        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = CancelReasonMinLength)]
        public string Reason { get; set; } = string.Empty;

        public string? Title { get; set; }
        public string? StartsAt { get; set; }
        public string? VenueName { get; set; }
        public int? JoinedCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var trimmedLength = (Reason ?? string.Empty).Trim().Length;

            if (trimmedLength < CancelReasonMinLength)
            {
                yield return new ValidationResult(
                    $"Give a reason of at least {CancelReasonMinLength} characters, not counting spaces at the ends.",
                    new[] { nameof(Reason) });
            }
        }
    }
}