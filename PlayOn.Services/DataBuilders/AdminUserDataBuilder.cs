using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminUsers;
using PlayOn.Services.Data.AdminUsers.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds user projections and response models for administration.
/// </summary>
public static class AdminUserDataBuilder
{
    /// <summary>
    /// Creates a query projection from a user to an admin user row.
    /// </summary>
    /// <returns>An expression that projects a user to an admin user row.</returns>
    internal static Expression<Func<ApplicationUser, AdminUserRow>>
        UserProjection()
    {
        return source => new AdminUserRow
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            Email = source.Email,
            City = source.City,
            IsActive = source.IsActive,
            MatchesPlayed = source.MatchesPlayed,
            ReliabilityScore = source.ReliabilityScore
        };
    }

    /// <summary>
    /// Maps a user row and its administrator flag to a response model.
    /// </summary>
    /// <param name="source">The user row.</param>
    /// <param name="isAdministrator">Whether the user holds the administrator role.</param>
    /// <returns>The user response model.</returns>
    internal static AdminUserResponseModel
        UserToResponseModel(
            AdminUserRow source,
            bool isAdministrator)
    {
        var result = new AdminUserResponseModel
        {
            Id = source.Id,
            DisplayName = source.DisplayName,
            Email = source.Email,
            City = source.City,
            IsActive = source.IsActive,
            IsAdministrator = isAdministrator,
            MatchesPlayed = source.MatchesPlayed,
            ReliabilityScore = source.ReliabilityScore
        };

        return result;
    }
}
