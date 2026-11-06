using static PlayOn.Common.ApplicationConstants.PaginationConstants;
namespace PlayOn.Services.Repositories.Data.Input
{
    public class VenueQueryDto
    {

        public int Page { get; set; } = FirstPage;
        public int PageSize { get; set; } = DefaultPageSize;

    }
}
