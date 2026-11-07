using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Contracts
{
    public interface ISportService
    {
        Task<ServiceResponse<IEnumerable<SportsResponseModel>>>
           GetSportsAsync();
    }
}
