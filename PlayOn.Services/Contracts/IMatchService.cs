using PlayOn.Common.Helpers;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Data.Matches.RequestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Contracts
{
    public interface IMatchService
    {
        Task<ServiceResponse<PagedResult<MatchCardResponseModel>>> GetMatchCardsAsync(
            MatchListRequestData request);
    }
}
