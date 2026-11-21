using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Attendance.RequestData
{
    public class AttendanceRequestData
    {
        public Guid MatchId { get; set; }

        [Required]
        public Guid ActorId { get; set; }

        public bool ActorIsAdmin { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Mark attendance for every player.")]
        public List<AttendanceEntryRequestData> Entries { get; set; } = [];
    }

    public class AttendanceEntryRequestData
    {
        [Required]
        public Guid UserId { get; set; }

        public bool Attended { get; set; }
    }
}
