using PlayOn.Services.Data.Ratings.RequestData;
using PlayOn.Services.Data.Ratings.ResponseModels;
using PlayOn.Web.ViewModels.Ratings;

namespace PlayOn.Web.DataBuilders;

public class RatingDataBuilder
{
    public RatingFormViewModel
        RatingFormToViewModel(RatingFormResponseModel source)
    {
        var result = new RatingFormViewModel
        {
            MatchId = source.MatchId,
            MatchTitle = source.MatchTitle,
            Players = source.Players
                .Select(RatingPlayerToViewModel)
                .ToList()
        };

        return result;
    }

    public RatingPlayerInputModel
        RatingPlayerToViewModel(RatingPlayerResponseModel source)
    {
        var result = new RatingPlayerInputModel
        {
            UserId = source.UserId,
            DisplayName = source.DisplayName,
            City = source.City
        };

        return result;
    }

    /// <summary>
    /// Refills the read-only fields of a posted rating form so it can be shown again with the typed values.
    /// </summary>
    public RatingFormViewModel
        RatingFormApplyDisplayData(
            RatingFormViewModel target,
            RatingFormResponseModel source)
    {
        target.MatchTitle = source.MatchTitle;

        foreach (var player in target.Players)
        {
            var match = source.Players.FirstOrDefault(x => x.UserId == player.UserId);

            if (match != null)
            {
                player.DisplayName = match.DisplayName;
                player.City = match.City;
            }
        }

        return target;
    }

    public RatingRequestData
        RatingFormToRequestData(
            Guid raterId,
            RatingFormViewModel source)
    {
        var result = new RatingRequestData
        {
            MatchId = source.MatchId,
            RaterId = raterId,
            Entries = source.Players
                .Where(x => x.Score.HasValue)
                .Select(x => new RatingEntryRequestData
                {
                    RatedUserId = x.UserId,
                    Score = x.Score!.Value,
                    Comment = x.Comment
                })
                .ToList()
        };

        return result;
    }
}
