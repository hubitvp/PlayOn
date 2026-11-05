namespace PlayOn.Services.Data.Home.RequestData
{
    using static PlayOn.Common.ApplicationConstants.PaginationConstants;

    public class HomeQueryRequestData
    {
        public int? SportId { get; set; }

        public string? City { get; set; }

        public DateOnly? From { get; set; }

        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
