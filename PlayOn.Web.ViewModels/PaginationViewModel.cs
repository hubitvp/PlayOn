namespace PlayOn.Web.ViewModels
{
    public class PaginationViewModel
    {
        public required int Page { get; init; }

        public required int TotalPages { get; init; }


        public required Func<int, string> PageUrl { get; init; }
    }
}
