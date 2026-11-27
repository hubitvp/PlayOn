using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match
{

    /// <summary>
    /// Query row (projection) holding all data needed to build the match details page.
    /// </summary>
    public sealed class DetailsRow
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Optional description of the match.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public MatchStatus Status { get; set; }
        /// <summary>
        /// Reason given when the match was cancelled, if any.
        /// </summary>
        public string? CancelReason { get; set; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }
        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        public int MinPlayers { get; set; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public int MaxPlayers { get; set; }
        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        public SkillLevel MinSkill { get; set; }
        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        public SkillLevel MaxSkill { get; set; }
        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        public decimal PricePerPlayer { get; set; }
        /// <summary>
        /// Identifier of the user who organises the match.
        /// </summary>
        public Guid OrganiserId { get; set; }
        /// <summary>
        /// Display name of the user who organises the match.
        /// </summary>
        public string OrganiserName { get; set; } = null!;
        /// <summary>
        /// Reliability score of the organiser, if one has been calculated.
        /// </summary>
        public int? OrganiserReliability { get; set; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string SportName { get; set; } = null!;
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public SportIcon SportIcon { get; set; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public string CourtName { get; set; } = null!;
        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        public Surface Surface { get; set; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public int CourtCapacity { get; set; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string VenueName { get; set; } = null!;
        /// <summary>
        /// Street address of the venue.
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Date on which the match takes place.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public TimeOnly ClosesAt { get; set; }
        /// <summary>
        /// Participants of the match.
        /// </summary>
        public List<ParticipantRow> Participants { get; set; } = [];
    }

    /// <summary>
    /// Query row (projection) describing a participant of a match.
    /// </summary>
    public sealed class ParticipantRow
    {
        /// <summary>
        /// Identifier of the participant.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Display name of the participant.
        /// </summary>
        public string DisplayName { get; set; } = null!;
        /// <summary>
        /// City of the participant.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Participation status of the user (e.g. joined or waitlisted).
        /// </summary>
        public ParticipantStatus Status { get; set; }
        /// <summary>
        /// Reliability score of the participant, if one has been calculated.
        /// </summary>
        public int? Reliability { get; set; }
    }

    /// <summary>
    /// Query row (projection) with the read-only data displayed next to the match edit form.
    /// </summary>
    public sealed class EditRowDisplayData
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string SportName { get; set; } = null!;
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public SportIcon SportIcon { get; set; }
        /// <summary>
        /// Identifier of the court.
        /// </summary>
        public Guid CourtId { get; set; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public string CourtName { get; set; } = null!;
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public int CourtCapacity { get; set; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string VenueName { get; set; } = null!;
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the venue slot starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public TimeOnly ClosesAt { get; set; }
        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public int JoinedCount { get; set; }
    }


    /// <summary>
    /// Query row (projection) holding the data needed to edit a match.
    /// </summary>
    public sealed class EditRow
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Identifier of the user who organises the match.
        /// </summary>
        public Guid OrganiserId { get; set; }
        /// <summary>
        /// Soft-delete flag indicating whether the match has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Current status of the match.
        /// </summary>
        public MatchStatus Status { get; set; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public string Title { get; set; } = null!;
        /// <summary>
        /// Optional description of the match.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }
        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        public int MinPlayers { get; set; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public int MaxPlayers { get; set; }
        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        public SkillLevel MinSkill { get; set; }
        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        public SkillLevel MaxSkill { get; set; }
        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        public decimal PricePerPlayer { get; set; }
        /// <summary>
        /// Concurrency token used to detect conflicting updates of the match.
        /// </summary>
        public byte[] RowVersion { get; set; } = null!;

        /// <summary>
        /// Read-only data displayed next to the edit form.
        /// </summary>
        public EditRowDisplayData EditRowDisplayData { get; set; } = null!;
    }

    /// <summary>
    /// Query row (projection) describing a venue slot and whether it is available for booking.
    /// </summary>
    public sealed class SlotRow
    {
        /// <summary>
        /// Unique identifier of the venue slot.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the venue slot starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Time the venue opens.
        /// </summary>
        public TimeOnly OpensAt { get; set; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public TimeOnly ClosesAt { get; set; }
        /// <summary>
        /// Indicates whether the slot is already taken by a match.
        /// </summary>
        public bool IsTaken { get; set; }
        /// <summary>
        /// Soft-delete flag indicating whether the venue has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
