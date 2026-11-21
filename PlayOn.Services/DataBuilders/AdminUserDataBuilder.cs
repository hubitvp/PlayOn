using PlayOn.Data.Models;
using PlayOn.Services.Data.AdminUsers;
using PlayOn.Services.Data.AdminUsers.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class AdminUserDataBuilder
{
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
