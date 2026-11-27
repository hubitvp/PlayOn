using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Venue.ResponseModels
{
    /// <summary>
    /// Response model describing a court of a venue.
    /// </summary>
    public class VenueCourtResponseModel
    {
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string Name { get; init; }
        /// <summary>
        /// Icon of the sport played on the court.
        /// </summary>
        public required SportIcon SportIcon { get; init; }
        /// <summary>
        /// Type of playing surface of the court.
        /// </summary>
        public required Surface Surface { get; init; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int Capacity { get; init; }
        /// <summary>
        /// Price for renting the court for one hour.
        /// </summary>
        public required decimal HourlyPrice { get; init; }
    }
}
