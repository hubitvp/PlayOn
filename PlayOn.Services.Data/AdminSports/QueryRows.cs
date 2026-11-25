using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.AdminSports
{
    public sealed class AdminSportRow
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public SportIcon Icon { get; set; }
        public int DefaultMinPlayers { get; set; }
        public int DefaultMaxPlayers { get; set; }
        public int CourtsCount { get; set; }
        public int MatchesCount { get; set; }
        public int PreferencesCount { get; set; }
    }
}
