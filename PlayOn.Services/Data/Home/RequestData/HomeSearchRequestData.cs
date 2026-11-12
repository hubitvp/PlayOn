using System;
using System.Collections.Generic;
using System.Text;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.Home.RequestData
{
    public class HomeSearchRequestData
    {
        public int? SportId { get; set; }

        public string? City { get; set; }

        public DateOnly? From { get; set; }

        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
