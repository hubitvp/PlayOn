using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchCancelRequestData : IValidatableObject
    {
        public Guid MatchId { get; set; }

        [Required]
        public Guid ActorId { get; set; }

        public bool ActorIsAdmin { get; set; }

        [Required]
        [StringLength(CancelReasonMaxLength, MinimumLength = 5)]
        public string Reason { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Reason.Trim().Length < 5)
            {
                yield return new ValidationResult(
                    "Give a reason of at least 5 characters, not counting spaces at the ends.",
                    new[] { nameof(Reason) });
            }
        }
    }
}
