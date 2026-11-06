using PlayOn.Common.Helpers;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;

namespace PlayOn.Services.Repositories.Contracts
{
    public interface IHomeRepository
    {
        Task<IEnumerable<SportDto>> GetSportsAsync();

        Task<PagedResult<MatchCardDto>>
                    GetMatchCardsAsync(MatchQueryDto matchQuery);

        Task<bool> SportExistsAsync(int sportId);
    }
}
