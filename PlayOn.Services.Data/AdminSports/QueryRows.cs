using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports
{
    /// <summary>
    /// Query row (projection) holding a sport and its related counts, used by the administrator sports list.
    /// </summary>
    public sealed class AdminSportRow
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Icon representing the sport.
        /// </summary>
        public SportIcon Icon { get; set; }
        /// <summary>
        /// Default minimum number of players for matches of the sport.
        /// </summary>
        public int DefaultMinPlayers { get; set; }
        /// <summary>
        /// Default maximum number of players for matches of the sport.
        /// </summary>
        public int DefaultMaxPlayers { get; set; }
        /// <summary>
        /// Number of courts on which the sport can be played.
        /// </summary>
        public int CourtsCount { get; set; }
        /// <summary>
        /// Number of matches organised for the sport.
        /// </summary>
        public int MatchesCount { get; set; }
        /// <summary>
        /// Number of users who have the sport among their preferences.
        /// </summary>
        public int PreferencesCount { get; set; }
    }
}
