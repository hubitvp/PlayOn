using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports.ResponseModels
{
    public class AdminSportEditResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required SportIcon Icon { get; init; }
        public required int DefaultMinPlayers { get; init; }
        public required int DefaultMaxPlayers { get; init; }
    }
}
