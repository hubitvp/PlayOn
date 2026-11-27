using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports.ResponseModels
{
    /// <summary>
    /// Response model describing a sport and its usage counts in the administrator sports list.
    /// </summary>
    public class AdminSportResponseModel
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
        /// <summary>
        /// Number of courts on which the sport can be played.
        /// </summary>
        public required int CourtsCount { get; init; }
        /// <summary>
        /// Number of matches organised for the sport.
        /// </summary>
        public required int MatchesCount { get; init; }
        /// <summary>
        /// Number of users who have the sport among their preferences.
        /// </summary>
        public required int PreferencesCount { get; init; }
    }
}
