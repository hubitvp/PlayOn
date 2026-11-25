using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchAttendanceInputModel
    {
        [NotEmptyGuid(ErrorMessage = "The player is not valid.")]
        public Guid UserId { get; set; }

        [ValidateNever]
        public string DisplayName { get; set; } = null!;

        [ValidateNever]
        public string City { get; set; } = null!;

        public bool Attended { get; set; } = true;
    }
}
