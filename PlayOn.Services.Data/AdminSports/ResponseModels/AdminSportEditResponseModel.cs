using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports.ResponseModels
{
    /// <summary>
    /// Response model holding the data needed to populate the edit form of a sport.
    /// </summary>
    public class AdminSportEditResponseModel
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string Name { get; init; }
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public required SportIcon Icon { get; init; }
        /// <summary>
        /// Default minimum number of players for matches of the sport.
        /// </summary>
        public required int DefaultMinPlayers { get; init; }
        /// <summary>
        /// Default maximum number of players for matches of the sport.
        /// </summary>
        public required int DefaultMaxPlayers { get; init; }
    }
}
