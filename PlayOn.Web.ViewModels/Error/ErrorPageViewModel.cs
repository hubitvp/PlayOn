namespace PlayOn.Web.ViewModels.Error
{
    public class ErrorPageViewModel
    {
        public required int Code { get; init; }
        public required string Title { get; init; }
        public required string Message { get; init; }
    }
}
