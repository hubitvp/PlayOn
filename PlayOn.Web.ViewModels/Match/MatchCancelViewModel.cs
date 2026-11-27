using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Model of the match cancellation form.
    /// </summary>
    public class MatchCancelViewModel : IValidatableObject
    {
        /// <summary>
        /// Identifier of the match to cancel.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// Reason given for cancelling the match.
        /// </summary>
        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = CancelReasonMinLength)]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Title of the match being cancelled.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Formatted date and time the match starts.
        /// </summary>
        public string? StartsAt { get; set; }
        /// <summary>
        /// Name of the venue where the match was to be played.
        /// </summary>
        public string? VenueName { get; set; }
        /// <summary>
        /// Number of players who have joined the match and will be notified.
        /// </summary>
        public int? JoinedCount { get; set; }

        /// <summary>
        /// Performs validation that cannot be expressed with attributes: the reason must have enough characters once surrounding spaces are trimmed.
        /// </summary>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>The validation errors found, if any.</returns>
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