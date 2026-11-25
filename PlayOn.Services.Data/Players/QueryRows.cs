namespace PlayOn.Services.Data.Players
{
    public sealed class PlayerRow
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string City { get; set; } = null!;
        public bool IsActive { get; set; }
        public int? ReliabilityScore { get; set; }
        public int MatchesPlayed { get; set; }
    }
}
