using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Home.ResponseModels;

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
            IEnumerable<SportsResponseModel> sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(x => new SportsResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return ServiceResponse<IEnumerable<SportsResponseModel>>
                .Success(sports);
        }

    }
}
