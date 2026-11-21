using System.ComponentModel.DataAnnotations;

namespace PlayOn.Services.Data.Ratings.RequestData
{
    public class RatingRequestData
    {
        public Guid MatchId { get; set; }

        [Required]
        public Guid RaterId { get; set; }

        public List<RatingEntryRequestData> Entries { get; set; } = [];
    }

    public class RatingEntryRequestData
    {
        public Guid RatedUserId { get; set; }

        public int Score { get; set; }

        public string? Comment { get; set; }
    }
}
