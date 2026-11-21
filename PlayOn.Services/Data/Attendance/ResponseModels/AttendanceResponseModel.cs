namespace PlayOn.Services.Data.Attendance.ResponseModels
{
    public class AttendanceResponseModel
    {
        public required Guid MatchId { get; init; }
        public required string Title { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required int DurationMinutes { get; init; }
        public required IReadOnlyList<AttendancePlayerResponseModel> Players { get; init; }
    }

    public class AttendancePlayerResponseModel
    {
        public required Guid UserId { get; init; }
        public required string DisplayName { get; init; }
        public required string City { get; init; }
    }
}
