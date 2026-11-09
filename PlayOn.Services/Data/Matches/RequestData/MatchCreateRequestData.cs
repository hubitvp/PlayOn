using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Matches.RequestData
{
    public class MatchCreateRequestData : MatchRequestDataBase
    {

        [Required]
        public string OrganiserId { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Select a sport.")]
        public int SportId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a court.")]
        public int CourtId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a date and time slot.")]
        public int VenueId { get; set; }
    }
}
