using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data;
using PlayOn.Services.Repositories.Contracts;
using PlayOn.Services.Repositories.Data.Input;
using PlayOn.Services.Repositories.Data.Output;

namespace PlayOn.Services.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly PlayOnDbContext context;
        public HomeRepository(PlayOnDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SportDto>> GetSportsAsync()
        {
            return await context.Sports
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new SportDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MatchCardDto>>
            GetMatchCardsAsync(MatchQueryDto matchQuery)
        {

            var query = context.Matches
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => !x.IsDeleted && !x.IsHidden)
                .Where(x => x.Status == MatchStatus.Open ||
                            x.Status == MatchStatus.Confirmed)
                .Where(x => x.Event != null)
                .Where(x => !x.Event!.Venue.IsDeleted);

            if (matchQuery.SportId.HasValue)
            {
                query = query.Where(x =>
                    x.Event!.SportId == matchQuery.SportId.Value);
            }

            if (!string.IsNullOrWhiteSpace(matchQuery.City))
            {
                query = query.Where(x =>
                    x.Event!.Venue.City.Contains(matchQuery.City)
                    || matchQuery.City.Contains(x.Event!.Venue.City));
            }

            if (matchQuery.From.HasValue)
            {
                query = query.Where(x =>
                    x.Event!.Venue.Date >= matchQuery.From.Value);
            }

            return await query
                .OrderBy(x => x.Event!.Venue.Date)
                .ThenBy(x => x.Event!.Venue.StartsAt)
                .ThenBy(x => x.Event!.Sport.Name)
                .ThenBy(x => x.Event!.Venue.Name)
                .Select(x => new MatchCardDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    SportName = x.Event!.Sport.Name,
                    SportIcon = x.Event!.Sport.Icon,
                    VenueName = x.Event!.Venue.Name,
                    City = x.Event!.Venue.City,
                    Date = x.Event!.Venue.Date,
                    StartsAt = x.Event!.Venue.StartsAt,
                    DurationMinutes = x.DurationMinutes,
                    Joined = x.Participants.Count(y =>
                        y.Status == ParticipantStatus.Joined),
                    MaxPlayers = x.MaxPlayers,
                    Waitlisted = x.Participants.Count(y =>
                        y.Status == ParticipantStatus.Waitlisted),
                    MinSkill = x.MinSkill,
                    PricePerPlayer = x.PricePerPlayer,
                    Status = x.Status
                })
                .ToListAsync();
        }



        public async Task<bool> SportExistsAsync(int sportId)
        {
            return await context.Sports
                .AnyAsync(x => x.Id == sportId);
        }
    }
}
