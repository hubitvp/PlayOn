
using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{

    /// <summary>
    /// A sport option offered when creating or searching matches.
    /// </summary>
    public class SportOptionResponseModel
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string Name { get; init; } = null!;
    }


    /// <summary>
    /// A court option offered when creating a match.
    /// </summary>
    public class CourtOptionResponseModel
    {
        /// <summary>
        /// Unique identifier of the court.
        /// </summary>
        public required Guid CourtId { get; init; }
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string Name { get; init; } = null!;
        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        public required Surface Surface { get; init; }

        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int Capacity { get; init; }

        /// <summary>
        /// Identifiers of the sports that can be played on the court.
        /// </summary>
        public required IReadOnlyCollection<Guid> SportIds { get; init; }
    }


    /// <summary>
    /// A venue (slot) option offered when creating a match.
    /// </summary>
    public class VenueOptionResponseModel
    {
        /// <summary>
        /// Unique identifier of the venue.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Name of the venue.
        /// </summary>
        public required string Name { get; init; }
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
    }

    /// <summary>
    /// The sport, court and venue options available when creating a match.
    /// </summary>
    public class MatchOptionsResponseModel
    {
        /// <summary>
        /// Sports available for selection.
        /// </summary>
        public required IReadOnlyList<SportOptionResponseModel> SportOptions { get; init; }
        /// <summary>
        /// Courts available for selection.
        /// </summary>
        public required IReadOnlyList<CourtOptionResponseModel> CourtOptions { get; init; }
        /// <summary>
        /// Venue slots available for selection.
        /// </summary>
        public required IReadOnlyList<VenueOptionResponseModel> VenueOptions { get; init; }
    }

}
