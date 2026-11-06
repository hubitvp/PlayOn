namespace PlayOn.Web.ViewModels.Venue
{
    public class CourtViewModel
    {
        public required string Name { get; init; }
        public string SportIconCss { get; set; } = string.Empty;
        public string Surface { get; set; } = string.Empty;
        public required int Capacity { get; init; }
        public string HourlyPrice { get; set; } = string.Empty;
    }
}
