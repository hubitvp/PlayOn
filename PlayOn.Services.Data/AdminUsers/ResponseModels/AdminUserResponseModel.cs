namespace PlayOn.Services.Data.AdminUsers.ResponseModels
{
    public class AdminUserResponseModel
    {
        public required Guid Id { get; init; }
        public required string DisplayName { get; init; }
        public string? Email { get; init; }
        public required string City { get; init; }
        public required bool IsActive { get; init; }
        public required bool IsAdministrator { get; init; }
        public required int MatchesPlayed { get; init; }
        public int? ReliabilityScore { get; init; }
    }
}
