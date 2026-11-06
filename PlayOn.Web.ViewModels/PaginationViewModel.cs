namespace PlayOn.Web.ViewModels
{
    public class PaginationViewModel
    {
        public int Page { get; init; }

        public int TotalPages { get; init; }


        public required Func<int, string> PageUrl { get; init; }
    }
}
