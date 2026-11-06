using static PlayOn.Common.ApplicationConstants.PaginationConstants;
namespace PlayOn.Web.ViewModels
{
    public class PagedViewModel
    {
        public int Page { get; init; } = FirstPage;
        public int PageSize { get; init; } = DefaultPageSize;
        public int TotalPages { get; init; } = 0;
    }
}
