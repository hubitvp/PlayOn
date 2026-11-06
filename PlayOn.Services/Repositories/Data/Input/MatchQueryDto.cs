using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Repositories.Data.Input
{
    public class MatchQueryDto
    {
        public int? SportId { get; set; }

        public string? City { get; set; }

        public DateOnly? From { get; set; }


        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
