namespace PlayOn.Web.ViewModels.Error;

public class ErrorPageViewModel
{
    public int Code { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
