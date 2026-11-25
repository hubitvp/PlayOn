namespace PlayOn.Services.Data.AdminUsers
{
    public sealed class AdminUserRow
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string? Email { get; set; }
        public string City { get; set; } = null!;
        public bool IsActive { get; set; }
        public int MatchesPlayed { get; set; }
        public int? ReliabilityScore { get; set; }
    }
}
