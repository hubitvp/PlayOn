using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Common.Helpers;
using PlayOn.Data;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PlayOn.Services.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly PlayOnDbContext context;
        public VenueRepository(PlayOnDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<PagedResult<VenueDto>>
            GetVenuesAsync(VenueQueryDto venueQuery)
        {

            var query = context.Venues
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Where(x => x.Event != null)
                .Where(x => x.Event!.Match.Status == MatchStatus.Open
                    || x.Event!.Match.Status == MatchStatus.Confirmed)
                .Where(x => x.Date > DateOnly.FromDateTime(DateTime.UtcNow));


            var totalCount = await query.CountAsync();

            var pageSize = Math.Clamp(venueQuery.PageSize, 1, 50);
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            var page = Math.Clamp(venueQuery.Page, 1, totalPages);


            var items = await query
                .OrderBy(x => x.Date)
                .ThenBy(x => x.OpensAt)
                .ThenBy(x => x.StartsAt)
                .ThenBy(x => x.ClosesAt)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.City)
                .ThenBy(x => x.Id)
                .Skip((venueQuery.Page - 1) * venueQuery.PageSize)
                .Take(venueQuery.PageSize)
                .Select(x => new VenueDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    City = x.City,
                    Date = x.Date,
                    StartsAt = x.StartsAt,
                    OpensAt = x.OpensAt,
                    ClosesAt = x.ClosesAt,
                    Court = new CourtDto
                    {
                        Name = x.Event!.Court.Name,
                        SportIcon = x.Event.Sport.Icon,
                        Surface = x.Event.Court.Surface,
                        Capacity = x.Event.Court.Capacity,
                        HourlyPrice = x.Event.Court.HourlyPrice
                    },
                    MatchId = x.Event.MatchId,
                    MatchTitle = x.Event.Match.Title,
                })
                .ToListAsync();

            return new PagedResult<VenueDto>(items, page, pageSize, totalCount);
        }
    }
}
