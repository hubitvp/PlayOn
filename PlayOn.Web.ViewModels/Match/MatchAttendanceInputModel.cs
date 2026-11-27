using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PlayOn.Common.Validation;

namespace PlayOn.Web.ViewModels.Match
{
    /// <summary>
    /// Attendance of a single player on the attendance form.
    /// </summary>
    public class MatchAttendanceInputModel
    {
        /// <summary>
        /// Identifier of the player.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The player is not valid.")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Display name of the player.
        /// </summary>
        [ValidateNever]
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// City of the player.
        /// </summary>
        [ValidateNever]
        public string City { get; set; } = null!;

        /// <summary>
        /// Indicates whether the player attended the match; <c>true</c> by default.
        /// </summary>
        public bool Attended { get; set; } = true;
    }
}
