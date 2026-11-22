namespace PlayOn.Web.ViewModels.Match
{


    public class CourtOptionViewModel
    {
        public required Guid CourtId { get; init; }
        public required string Name { get; init; }
        public required string Surface { get; init; }
        public required int Capacity { get; init; }
        public required IEnumerable<Guid> SportIds { get; init; }
    }

    public class VenueOptionViewModel
    {
        public required Guid Id { get; init; }
        public required string Label { get; init; }
        public required string Date { get; init; }
        public required int StartMinutes { get; init; }
        public required int CloseMinutes { get; init; }
    }

}
