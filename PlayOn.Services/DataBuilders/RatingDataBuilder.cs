using PlayOn.Data.Models;
using PlayOn.Services.Data.Ratings;
using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;
using System.Linq.Expressions;

namespace PlayOn.Services.DataBuilders;

/// <summary>
/// Builds projections, entities and response models for ratings.
/// </summary>
public static class RatingDataBuilder
{
    /// <summary>
    /// Creates a projection from a match to the information needed for rating.
    /// </summary>
    /// <returns>The projection expression.</returns>
    internal static Expression<Func<Match, RatingInfoRow>>
        RatingInfoProjection()
    {
        return source => new RatingInfoRow
        {
            Id = source.Id,
            Title = source.Title,
            Status = source.Status,
            IsDeleted = source.IsDeleted
        };
    }

    /// <summary>
    /// Creates a projection from a participation to a ratable player model.
    /// </summary>
    /// <returns>The projection expression.</returns>
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

    /// <summary>
    /// Creates the rating form response model.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="matchTitle">The title of the match.</param>
    /// <param name="players">The players that can be rated.</param>
    /// <returns>The rating form response model.</returns>
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

    /// <summary>
    /// Creates a visible rating entity from an entry; the comment is trimmed and blank comments become null.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="raterId">The identifier of the rating user.</param>
    /// <param name="source">The rating entry.</param>
    /// <returns>The new rating entity.</returns>
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
