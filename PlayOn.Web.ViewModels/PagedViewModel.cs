using System;
using System.Collections.Generic;
using System.Text;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;
namespace PlayOn.Web.ViewModels
{
    public class PagedViewModel
    {
        public required int Page { get; init; } = FirstPage;
        public required int PageSize { get; init; } = DefaultPageSize;
        public required int TotalPages { get; init; } = 0;
    }
}
