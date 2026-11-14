using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchCreateRequestData : MatchRequestDataBase
    {

        [Required]
        public Guid OrganiserId { get; set; }

        public Guid SportId { get; set; }

        public Guid CourtId { get; set; }

        public Guid VenueId { get; set; }
    }
}
