using System;
using System.Collections.Generic;
using System.Text;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.Venue.RequestData
{
    public class VenueQueryRequestData
    {
        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
