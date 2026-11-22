using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data
{
    public class PageRequestData
    {
        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
