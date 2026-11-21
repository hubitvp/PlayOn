using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Attendance
{
    internal sealed class AttendanceInfoRow
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid OrganiserId { get; set; }
        public MatchStatus Status { get; set; }
        public string Title { get; set; } = null!;
        public string VenueName { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public int DurationMinutes { get; set; }
    }
}
