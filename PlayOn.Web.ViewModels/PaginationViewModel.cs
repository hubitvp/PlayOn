namespace PlayOn.Web.ViewModels
{
    /// <summary>
    /// View model of the pagination control.
    /// </summary>
    public class PaginationViewModel
    {
        /// <summary>
        /// Number of the current page.
        /// </summary>
        public required int Page { get; init; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public required int TotalPages { get; init; }


        /// <summary>
        /// Function that builds the URL of the given page number.
        /// </summary>
        public required Func<int, string> PageUrl { get; init; }
    }
}
