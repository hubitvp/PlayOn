using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchUpdateRequestData : MatchRequestDataBase
    {
        [Required]
        public Guid MatchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool UserIsAdmin { get; set; }

        [Required]
        public byte[] RowVersion { get; set; } = null!;
    }
}
