namespace PlayOn.Web.ViewModels.Error
{
    /// <summary>
    /// View model of the error page.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Numeric error code, such as an HTTP status code.
        /// </summary>
        public required int Code { get; init; }
        /// <summary>
        /// Short title of the error.
        /// </summary>
        public required string Title { get; init; }
        /// <summary>
        /// Description of the error shown to the user.
        /// </summary>
        public required string Message { get; init; }
    }
}