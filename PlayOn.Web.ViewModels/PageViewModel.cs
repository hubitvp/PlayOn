using static PlayOn.Common.ApplicationConstants.PaginationConstants;
namespace PlayOn.Web.ViewModels
{
    /// <summary>
    /// Base view model holding the paging information of a list.
    /// </summary>
    public class PageViewModel
    {
        /// <summary>
        /// Number of the current page.
        /// </summary>
        public int Page { get; init; } = FirstPage;
        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; init; } = DefaultPageSize;
        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; init; } = 0;
    }
}
