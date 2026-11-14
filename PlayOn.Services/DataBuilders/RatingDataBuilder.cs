using PlayOn.Data.Models;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

public static class RatingDataBuilder
{
    public static Expression<Func<MatchParticipant, RatingPlayerResponseModel>>
        RatingPlayerProjection()
    {
        return source => new RatingPlayerResponseModel
        {
            UserId = source.UserId,
            DisplayName = source.User.DisplayName,
            City = source.User.City
        };
    }

    public static RatingFormResponseModel
        RatingFormToResponseModel(
            Guid matchId,
            string matchTitle,
            IReadOnlyList<RatingPlayerResponseModel> players)
    {
        var result = new RatingFormResponseModel
        {
            MatchId = matchId,
            MatchTitle = matchTitle,
            Players = players
        };

        return result;
    }

    public static PlayerRating
        RatingToEntity(
            Guid matchId,
            Guid raterId,
            RatingEntryRequestData source)
    {
        var comment = string.IsNullOrWhiteSpace(source.Comment)
            ? null
            : source.Comment.Trim();

        var result = new PlayerRating
        {
            MatchId = matchId,
            RaterId = raterId,
            RatedUserId = source.RatedUserId,
            Score = source.Score,
            Comment = comment,
            IsHidden = false
        };

        return result;
    }
}
