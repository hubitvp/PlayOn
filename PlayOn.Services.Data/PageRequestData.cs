using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data
{
    /// <summary>
    /// Request data holding the paging parameters.
    /// </summary>
    public class PageRequestData
    {
        /// <summary>
        /// Number of the requested page.
        /// </summary>
        public int Page { get; set; } = FirstPage;
        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
