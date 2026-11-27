
namespace PlayOn.Services.Data.Home.RequestData
{

    /// <summary>
    /// Request data for filtering the matches shown on the home page.
    /// </summary>
    public class HomeSearchRequestData
    {
        /// <summary>
        /// Optional sport used to filter the matches.
        /// </summary>
        public Guid? SportId { get; set; }

        /// <summary>
        /// Optional city used to filter the matches.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Optional date from which matches are shown.
        /// </summary>
        public DateOnly? From { get; set; }
    }
}
