namespace PlayOn.Web.ViewModels.Admin
{
    public class AdminUserItemViewModel
    {
        public required Guid Id { get; init; }
        public required string DisplayName { get; init; }
        public required string Email { get; init; }
        public required string City { get; init; }
        public required bool IsActive { get; init; }
        public required bool IsAdministrator { get; init; }
        public required bool IsCurrentUser { get; init; }
        public required int MatchesPlayed { get; init; }
        public required string Reliability { get; init; }
    }
}
