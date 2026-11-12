using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
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

        public async Task<ServiceResponse<IEnumerable<SportResponseModel>>>
            GetSportsAsync()
        {
            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .ToListAsync();

            IEnumerable<SportResponseModel> response = sports
                .Select(SportDataBuilder.SportToResponseModel)
                .ToList();

            return ServiceResponse<IEnumerable<SportResponseModel>>
                .Success(response);
        }

    }
}
