using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Venue.ResponseModels
{
    public class CourtResponseModel
    {
        public required string Name { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required Surface Surface { get; init; }
        public required int Capacity { get; init; }
        public required decimal HourlyPrice { get; init; }
    }
}
