namespace PlayOn.Web.ViewModels.Venue
{
    /// <summary>
    /// A court of a venue shown on the venues page.
    /// </summary>
    public class VenueCourtViewModel
    {
        /// <summary>
        /// Name of the court.
        /// </summary>
        public required string Name { get; init; }
        /// <summary>
        /// CSS class of the icon representing the sport.
        /// </summary>
        public required string SportIconCss { get; init; }
        /// <summary>
        /// Display text of the playing surface of the court.
        /// </summary>
        public required string Surface { get; init; }
        /// <summary>
        /// Maximum number of people the court can accommodate.
        /// </summary>
        public required int Capacity { get; init; }
        /// <summary>
        /// Formatted price for renting the court for one hour.
        /// </summary>
        public required string HourlyPrice { get; init; }
    }
}
