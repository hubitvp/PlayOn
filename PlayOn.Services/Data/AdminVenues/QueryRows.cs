namespace PlayOn.Services.Data.AdminVenues
{
    internal sealed class AdminVenueRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeOnly OpensAt { get; set; }
        public TimeOnly ClosesAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBooked { get; set; }
        public string? MatchTitle { get; set; }
    }
}
