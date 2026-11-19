namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminSportItemViewModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string IconCss { get; init; }
        public required int DefaultMinPlayers { get; init; }
        public required int DefaultMaxPlayers { get; init; }
        public required int CourtsCount { get; init; }
        public required int MatchesCount { get; init; }
        public required int PreferencesCount { get; init; }
        public required bool CanDelete { get; init; }
    }
}
