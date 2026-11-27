using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    /// <summary>
    /// Request data for updating an existing match.
    /// </summary>
    public class MatchUpdateRequestData : MatchRequestDataBase
    {
        /// <summary>
        /// Identifier of the match to update.
        /// </summary>
        [Required]
        public Guid MatchId { get; set; }

        /// <summary>
        /// Identifier of the user updating the match.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Indicates whether the acting user is an administrator.
        /// </summary>
        [Required]
        public bool UserIsAdmin { get; set; }

        /// <summary>
        /// Concurrency token the client received when loading the match, used to detect conflicting updates.
        /// </summary>
        [Required]
        public byte[] RowVersion { get; set; } = null!;
    }
}
