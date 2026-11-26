using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Participation
{
    public sealed class JoinInfoRow
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public MatchStatus Status { get; set; }
        public string Title { get; set; } = null!;
        public Guid OrganiserId { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public SkillLevel MinSkill { get; set; }
        public SkillLevel MaxSkill { get; set; }
        public int DurationMinutes { get; set; }
        public Guid SportId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
    }

    public sealed class BookingRow
    {
        public TimeOnly StartsAt { get; set; }
        public int DurationMinutes { get; set; }
    }
}
