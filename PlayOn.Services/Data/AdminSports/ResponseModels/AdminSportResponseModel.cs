using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports.ResponseModels
{
    public class AdminSportResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required SportIcon Icon { get; init; }
        public required int DefaultMinPlayers { get; init; }
        public required int DefaultMaxPlayers { get; init; }
        public required int CourtsCount { get; init; }
        public required int MatchesCount { get; init; }
        public required int PreferencesCount { get; init; }
    }
}
