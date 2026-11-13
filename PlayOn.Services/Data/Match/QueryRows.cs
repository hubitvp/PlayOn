using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match
{

    public sealed class DetailsRow
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public MatchStatus Status { get; set; }
        public string? CancelReason { get; set; }
        public bool IsHidden { get; set; }
        public int DurationMinutes { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public SkillLevel MinSkill { get; set; }
        public SkillLevel MaxSkill { get; set; }
        public decimal PricePerPlayer { get; set; }
        public Guid OrganiserId { get; set; }
        public string OrganiserName { get; set; } = null!;
        public int? OrganiserReliability { get; set; }
        public string SportName { get; set; } = null!;
        public SportIcon SportIcon { get; set; }
        public string CourtName { get; set; } = null!;
        public Surface Surface { get; set; }
        public int CourtCapacity { get; set; }
        public string VenueName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeOnly ClosesAt { get; set; }
        public List<ParticipantRow> Participants { get; set; } = [];
    }

    public sealed class ParticipantRow
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = null!;
        public string City { get; set; } = null!;
        public ParticipantStatus Status { get; set; }
        public int? Reliability { get; set; }
    }

    internal sealed class EditRow
    {
        public Guid Id { get; set; }
        public Guid OrganiserId { get; set; }
        public bool IsDeleted { get; set; }
        public MatchStatus Status { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public SkillLevel MinSkill { get; set; }
        public SkillLevel MaxSkill { get; set; }
        public decimal PricePerPlayer { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public string SportName { get; set; } = null!;
        public SportIcon SportIcon { get; set; }
        public Guid CourtId { get; set; }
        public string CourtName { get; set; } = null!;
        public int CourtCapacity { get; set; }
        public string VenueName { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeOnly ClosesAt { get; set; }
        public int JoinedCount { get; set; }
    }

    internal sealed class SlotRow
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartsAt { get; set; }
        public TimeOnly OpensAt { get; set; }
        public TimeOnly ClosesAt { get; set; }
        public bool IsTaken { get; set; }
        public bool IsDeleted { get; set; }
    }

    internal sealed class BookingRow
    {
        public TimeOnly StartsAt { get; set; }
        public int DurationMinutes { get; set; }
    }
}
