using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Attendance.RequestData
{
    /// <summary>
    /// Request data for recording which players attended a match.
    /// </summary>
    public class AttendanceRequestData
    {
        /// <summary>
        /// Identifier of the match to record attendance for.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// Identifier of the user recording the attendance.
        /// </summary>
        [Required]
        public Guid ActorId { get; set; }

        /// <summary>
        /// Indicates whether the acting user is an administrator.
        /// </summary>
        public bool ActorIsAdmin { get; set; }

        /// <summary>
        /// Attendance entries, one per player.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "Mark attendance for every player.")]
        public List<AttendanceEntryRequestData> Entries { get; set; } = [];
    }

    /// <summary>
    /// Attendance of a single player within an <see cref="AttendanceRequestData"/>.
    /// </summary>
    public class AttendanceEntryRequestData
    {
        /// <summary>
        /// Identifier of the player.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Indicates whether the player attended the match.
        /// </summary>
        public bool Attended { get; set; }
    }
}
