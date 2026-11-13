using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchEditViewModel : MatchInputModelBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "The match is not valid.")]
        public Guid Id { get; set; }

        [MinLength(1, ErrorMessage = "This page is out of date. Open the match again.")]
        public byte[] RowVersion { get; set; } = [];

        public string SportName { get; set; } = null!;
        public string SportIcon { get; set; } = null!;
        public string CourtName { get; set; } = null!;
        public int CourtCapacity { get; set; }
        public string VenueName { get; set; } = null!;
        public string City { get; set; } = null!;
        public string StartsAt { get; set; } = null!;
        public int JoinedCount { get; set; }
    }
}
