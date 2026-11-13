using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchCreateRequestData : MatchRequestDataBase
    {

        [Required]
        public Guid OrganiserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a sport.")]
        public Guid SportId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a court.")]
        public Guid CourtId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a date and time slot.")]
        public Guid VenueId { get; set; }
    }
}
