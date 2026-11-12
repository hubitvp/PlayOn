using PlayOn.Data.Models;
using PlayOn.Services.Data.Sport.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class SportDataBuilder
{
    public static Expression<Func<Sport, SportResponseModel>>
        SportProjection()
    {
        return source => new SportResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };
    }

    public static SportResponseModel
        SportToResponseModel(Sport source)
    {
        var result = new SportResponseModel
        {
            Id = source.Id,
            Name = source.Name
        };

        return result;
    }
}
