using PlayOn.Data.Models;
using PlayOn.Services.Data.Sport.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Provides projections that map sport entities to response models.
/// </summary>
public static class SportDataBuilder
{
    /// <summary>
    /// Creates a projection from a sport to a response model containing its id and name.
    /// </summary>
    /// <returns>An expression projecting a <see cref="Sport"/> to a <see cref="SportResponseModel"/>.</returns>
    public static Expression<Func<Sport, SportResponseModel>>
        SportProjection()
    {
        return source => new SportResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }
}
