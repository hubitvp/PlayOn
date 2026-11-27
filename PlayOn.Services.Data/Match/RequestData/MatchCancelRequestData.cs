using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Services.Data.Match.RequestData
{
    /// <summary>
    /// Request data for cancelling a match.
    /// </summary>
    public class MatchCancelRequestData : IValidatableObject
    {
        /// <summary>
        /// Identifier of the match to cancel.
        /// </summary>
        [Required]
        public Guid MatchId { get; set; }

        /// <summary>
        /// Identifier of the user cancelling the match.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Indicates whether the acting user is an administrator.
        /// </summary>
        [Required]
        public bool UserIsAdmin { get; set; }

        /// <summary>
        /// Reason given for cancelling the match.
        /// </summary>
        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = CancelReasonMinLength)]
        public string Reason { get; set; } = null!;

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the reason must have enough characters once surrounding spaces are trimmed.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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
