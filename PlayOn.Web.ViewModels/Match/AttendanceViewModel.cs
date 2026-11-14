using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    public class AttendanceViewModel
    {
        [NotEmptyGuid(ErrorMessage = "The match is not valid.")]
        public Guid MatchId { get; set; }

        [ValidateNever]
        public string Title { get; set; } = string.Empty;

        [ValidateNever]
        public string StartsAt { get; set; } = string.Empty;

        [ValidateNever]
        public string VenueName { get; set; } = string.Empty;

        public List<AttendancePlayerInputModel> Players { get; set; } = [];
    }
}
