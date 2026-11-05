using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;

namespace PlayOn.Services.Repositories.Contracts
{
    public interface IHomeRepository
    {
        Task<IEnumerable<SportDto>> GetSportsAsync();

        Task<IEnumerable<MatchCardDto>>
            GetMatchCardsAsync(MatchQueryDto matchQuery);

        Task<bool> SportExistsAsync(int sportId);
    }
}
