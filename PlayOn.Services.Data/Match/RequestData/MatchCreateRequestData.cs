using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    /// <summary>
    /// Request data for creating a new match.
    /// </summary>
    public class MatchCreateRequestData : MatchRequestDataBase
    {

        /// <summary>
        /// Identifier of the user creating and organising the match.
        /// </summary>
        [Required]
        public Guid OrganiserId { get; set; }

        /// <summary>
        /// Identifier of the sport to be played.
        /// </summary>
        [Required]
        public Guid SportId { get; set; }

        /// <summary>
        /// Identifier of the court where the match is played.
        /// </summary>
        [Required]
        public Guid CourtId { get; set; }

        /// <summary>
        /// Identifier of the venue slot booked for the match.
        /// </summary>
        [Required]
        public Guid VenueId { get; set; }
    }
}
