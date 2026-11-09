namespace PlayOn.Web.ViewModels.Match
{
    public class CourtOptionViewModel
    {
        public required int CourtId { get; init; }
        public required string Name { get; init; }
        public required string Surface { get; init; }
        public required int Capacity { get; init; }
        public required int[] SportIds { get; init; }
    }

    public class VenueSlotOptionViewModel
    {
        public required int Id { get; init; }
        public required string Label { get; init; }
        public required string Date { get; init; }
        public required int StartMinutes { get; init; }
        public required int CloseMinutes { get; init; }
    }
}
