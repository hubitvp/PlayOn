using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminVenues.RequestData
{
    public class AdminVenueUpdateRequestData : AdminVenueRequestDataBase
    {
        [NotEmptyGuid(ErrorMessage = "The venue is not valid.")]
        public Guid VenueId { get; set; }
    }
}
