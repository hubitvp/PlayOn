using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    public class AttendancePlayerInputModel
    {
        [NotEmptyGuid(ErrorMessage = "The player is not valid.")]
        public Guid UserId { get; set; }

        [ValidateNever]
        public string DisplayName { get; set; } = string.Empty;

        [ValidateNever]
        public string City { get; set; } = string.Empty;

        public bool Attended { get; set; } = true;
    }
}
