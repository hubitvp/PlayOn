using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Participation
{
    /// <summary>
    /// Query row (projection) with the match information needed when a user joins or leaves a match.
    /// </summary>
    public sealed class JoinInfoRow
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public Guid Id { get; set; }
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
        /// Identifier of the user who organises the match.
        /// </summary>
        public Guid OrganiserId { get; set; }
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
        /// Duration of the match in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }
        /// <summary>
        /// Identifier of the sport of the match.
        /// </summary>
        public Guid SportId { get; set; }
        /// <summary>
        /// Date on which the match takes place.
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// Time the match starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
    }

    /// <summary>
    /// Query row (projection) with the time of a match the user is already booked for, used to detect overlaps.
    /// </summary>
    public sealed class BookingRow
    {
        /// <summary>
        /// Time the match the user is booked for starts.
        /// </summary>
        public TimeOnly StartsAt { get; set; }
        /// <summary>
        /// Duration of the match the user is booked for, in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }
    }
}
