using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;
using PlayOn.Services.Mappers;

namespace PlayOn.Services
{
    public class SportService : ISportService
    {
        private readonly IRepository<Sport> sportRepository;

        public SportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        public async Task<ServiceResponse<IEnumerable<SportsResponseModel>>>
            GetSportsAsync()
        {
            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .ToListAsync();

            IEnumerable<SportsResponseModel> response = sports
                .Select(HomeMapper.ToSportsResponse)
                .ToList();

            return ServiceResponse<IEnumerable<SportsResponseModel>>
                .Success(response);
        }

    }
}
