using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Matches.RequestData
{
    public class MatchUpdateRequestData : MatchRequestDataBase
    {
        public int MatchId { get; set; }

        [Required]
        public string ActorId { get; set; } = null!;

        public bool ActorIsAdmin { get; set; }

        [Required]
        public byte[] RowVersion { get; set; } = null!;
    }
}
