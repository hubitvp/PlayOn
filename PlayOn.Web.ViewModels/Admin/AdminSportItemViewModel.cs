namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// A sport shown as a row in the administrator sports list.
    /// </summary>
    public class AdminSportItemViewModel
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
        /// CSS class of the icon representing the sport.
        /// </summary>
        public required string IconCss { get; init; }
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
        /// <summary>
        /// Indicates whether the sport can be deleted.
        /// </summary>
        public required bool CanDelete { get; init; }
    }
}
