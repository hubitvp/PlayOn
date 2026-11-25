using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Sport.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    public class SportService : ISportService
    {
        private readonly IRepository<Sport> sportRepository;

        public SportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        public async Task<ServiceResponse<IReadOnlyList<SportResponseModel>>>
            GetSportOptionsAsync()
        {
            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(SportDataBuilder.SportProjection())
                .ToListAsync();

            return ServiceResponse<IReadOnlyList<SportResponseModel>>
                .Success(sports);
        }
    }
}
