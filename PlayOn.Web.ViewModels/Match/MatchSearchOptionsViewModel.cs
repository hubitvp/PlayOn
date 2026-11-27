namespace PlayOn.Web.ViewModels.Match
{


    /// <summary>
    /// A court option offered when creating a match.
    /// </summary>
    public class CourtOptionViewModel
    {
        /// <summary>
        /// Identifier of the court.
        /// </summary>
        public required Guid CourtId { get; init; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string Name { get; init; }
        /// <summary>
        /// Display text of the playing surface of the court.
        /// </summary>
        public required string Surface { get; init; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int Capacity { get; init; }
        /// <summary>
        /// Identifiers of the sports that can be played on the court.
        /// </summary>
        public required IEnumerable<Guid> SportIds { get; init; }
    }

    /// <summary>
    /// A venue slot option offered when creating a match.
    /// </summary>
    public class VenueOptionViewModel
    {
        /// <summary>
        /// Unique identifier of the venue slot.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Display label of the venue slot.
        /// </summary>
        public required string Label { get; init; }
        /// <summary>
        /// Formatted date of the venue slot.
        /// </summary>
        public required string Date { get; init; }
        /// <summary>
        /// Time the slot starts, expressed in minutes since midnight.
        /// </summary>
        public required int StartMinutes { get; init; }
        /// <summary>
        /// Time the venue closes, expressed in minutes since midnight.
        /// </summary>
        public required int CloseMinutes { get; init; }
    }

}
