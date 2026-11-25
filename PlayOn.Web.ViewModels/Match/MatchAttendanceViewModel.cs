using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchAttendanceViewModel
    {
        [NotEmptyGuid(ErrorMessage = "The match is not valid.")]
        public Guid MatchId { get; set; }

        [ValidateNever]
        public string Title { get; set; } = null!;

        [ValidateNever]
        public string StartsAt { get; set; } = null!;

        [ValidateNever]
        public string VenueName { get; set; } = null!;

        public List<MatchAttendanceInputModel> Players { get; set; } = [];
    }
}
