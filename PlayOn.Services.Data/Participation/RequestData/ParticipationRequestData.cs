using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Participation.RequestData
{
    /// <summary>
    /// Request data for joining or leaving a match.
    /// </summary>
    public class ParticipationRequestData
    {
        /// <summary>
        /// Identifier of the match to join or leave.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// Identifier of the user joining or leaving the match.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// Request data for listing the matches of a user.
    /// </summary>
    public class MyMatchesRequestData
    {
        /// <summary>
        /// Identifier of the user whose matches are listed.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// The tab (e.g. upcoming or past) selecting which of the user's matches to list.
        /// </summary>
        [EnumDataType(typeof(MyMatchesTab))]
        public MyMatchesTab Tab { get; set; } = MyMatchesTab.Upcoming;
    }
}
