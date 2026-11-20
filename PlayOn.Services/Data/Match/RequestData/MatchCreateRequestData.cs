using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchCreateRequestData : MatchRequestDataBase
    {

        [Required]
        public Guid OrganiserId { get; set; }

        [Required]
        public Guid SportId { get; set; }

        [Required]
        public Guid CourtId { get; set; }

        [Required]
        public Guid VenueId { get; set; }
    }
}
