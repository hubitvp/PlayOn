namespace PlayOn.Web.ViewModels.Match
{
    public class MatchSlotOptionViewModel
    {
        public required Guid Id { get; init; }
        public required string Label { get; init; }
        public required string Date { get; init; }
        public required int StartMinutes { get; init; }
        public required int CloseMinutes { get; init; }
    }
}
