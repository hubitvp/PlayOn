namespace PlayOn.Web.ViewModels.Admin
{
    /// <summary>
    /// A user shown as a row in the administrator users list.
    /// </summary>
    public class AdminUserItemViewModel
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Display name of the user.
        /// </summary>
        public required string DisplayName { get; init; }
        /// <summary>
        /// Email address of the user.
        /// </summary>
        public required string Email { get; init; }
        /// <summary>
        /// City of the user.
        /// </summary>
        public required string City { get; init; }
        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public required bool IsActive { get; init; }
        /// <summary>
        /// Indicates whether the user has the administrator role.
        /// </summary>
        public required bool IsAdministrator { get; init; }
        /// <summary>
        /// Indicates whether the row represents the currently signed-in user.
        /// </summary>
        public required bool IsCurrentUser { get; init; }
        /// <summary>
        /// Total number of matches the user has played.
        /// </summary>
        public required int MatchesPlayed { get; init; }
        /// <summary>
        /// Formatted reliability score, if one has been calculated.
        /// </summary>
        public required string Reliability { get; init; }
    }
}
