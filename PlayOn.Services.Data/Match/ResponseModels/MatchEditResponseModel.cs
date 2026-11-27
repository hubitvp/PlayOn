using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{


    /// <summary>
    /// Read-only data displayed next to the match edit form.
    /// </summary>
    public class MatchEditDisplayDataResponseModel
    {
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string SportName { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required SportIcon SportIcon { get; init; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string CourtName { get; init; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int CourtCapacity { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string VenueName { get; init; }
        /// <summary>
        /// City in which the venue is located.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Date of the venue slot.
        /// </summary>
        public required DateOnly Date { get; init; }
        /// <summary>
        /// Time the venue slot starts.
        /// </summary>
        public required TimeOnly StartsAt { get; init; }
        /// <summary>
        /// Time the venue closes.
        /// </summary>
        public required TimeOnly ClosesAt { get; init; }

        /// <summary>
        /// Number of players who have joined the match.
        /// </summary>
        public required int JoinedCount { get; init; }
    }



    /// <summary>
    /// Response model holding the data needed to populate the match edit form.
    /// </summary>
    public class MatchEditResponseModel
    {
        /// <summary>
        /// Unique identifier of the match.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Title of the match.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Optional description of the match.
        /// </summary>
        public required string? Description { get; init; }
        /// <summary>
        /// Duration of the match in minutes.
        /// </summary>
        public required int DurationMinutes { get; init; }
        /// <summary>
        /// Minimum number of players required for the match to take place.
        /// </summary>
        public required int MinPlayers { get; init; }
        /// <summary>
        /// Maximum number of players allowed to join the match.
        /// </summary>
        public required int MaxPlayers { get; init; }
        /// <summary>
        /// Minimum skill level required to join the match.
        /// </summary>
        public required SkillLevel MinSkill { get; init; }
        /// <summary>
        /// Maximum skill level allowed to join the match.
        /// </summary>
        public required SkillLevel MaxSkill { get; init; }
        /// <summary>
        /// Price each player pays to join the match.
        /// </summary>
        public required decimal PricePerPlayer { get; init; }
        /// <summary>
        /// Concurrency token used to detect conflicting updates of the match.
        /// </summary>
        public required byte[] RowVersion { get; init; }


        /// <summary>
        /// Read-only data displayed next to the edit form.
        /// </summary>
        public required MatchEditDisplayDataResponseModel EditDisplayDataResponseModel { get; set; }

    }
}
