using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Model of the attendance form: the match details and the players to mark.
    /// </summary>
    public class MatchAttendanceViewModel
    {
        /// <summary>
        /// Identifier of the match attendance is taken for.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The match is not valid.")]
        public Guid MatchId { get; set; }

        /// <summary>
        /// Title of the match.
        /// </summary>
        [ValidateNever]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Formatted date and time the match starts.
        /// </summary>
        [ValidateNever]
        public string StartsAt { get; set; } = null!;

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [ValidateNever]
        public string VenueName { get; set; } = null!;

        /// <summary>
        /// Attendance inputs, one per player.
        /// </summary>
        public List<MatchAttendanceInputModel> Players { get; set; } = [];
    }
}
