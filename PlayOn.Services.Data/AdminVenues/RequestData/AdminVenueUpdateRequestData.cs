using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    /// <summary>
    /// Request data for updating an existing venue.
    /// </summary>
    public class AdminVenueUpdateRequestData : AdminVenueRequestDataBase
    {
        /// <summary>
        /// Identifier of the venue to update.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The venue is not valid.")]
        public Guid VenueId { get; set; }
    }
}
