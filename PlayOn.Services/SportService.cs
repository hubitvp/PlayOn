using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Sport.ResponseModels;
using PlayOn.Services.DataBuilders;

namespace PlayOn.Services
{
    /// <summary>
    /// Provides access to the available sports.
    /// </summary>
    public class SportService : ISportService
    {
        private readonly IRepository<Sport> sportRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SportService"/> class.
        /// </summary>
        /// <param name="sportRepository">The repository used to query sports.</param>
        public SportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        /// <summary>
        /// Retrieves all sports ordered by name.
        /// </summary>
        /// <returns>A successful response with the sports ordered by name.</returns>
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
