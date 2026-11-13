using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Match.RequestData
{
    public class MatchUpdateRequestData : MatchRequestDataBase
    {
        public Guid MatchId { get; set; }

        [Required]
        public Guid ActorId { get; set; }

        public bool ActorIsAdmin { get; set; }

        [Required]
        public byte[] RowVersion { get; set; } = null!;
    }
}
