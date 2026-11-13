namespace PlayOn.Web.ViewModels.Match
{
    public class MatchCourtOptionViewModel
    {
        public required Guid CourtId { get; init; }
        public required string Name { get; init; }
        public required string Surface { get; init; }
        public required int Capacity { get; init; }
        public required IEnumerable<Guid> SportIds { get; init; }
    }
}
