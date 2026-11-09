using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Match
{
    public class MatchEditViewModel : MatchInputModelBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "The match is not valid.")]
        public int Id { get; set; }

        /// <summary>The version the user opened. Posted back so a concurrent change is detected.</summary>
        [MinLength(1, ErrorMessage = "This page is out of date. Open the match again.")]
        public byte[] RowVersion { get; set; } = [];

        // Read-only summary of what cannot be changed. Filled by the controller for every render.
        public string SportName { get; set; } = string.Empty;
        public string SportIcon { get; set; } = string.Empty;
        public string CourtName { get; set; } = string.Empty;
        public int CourtCapacity { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StartsAt { get; set; } = string.Empty;
        public int JoinedCount { get; set; }
    }
}
