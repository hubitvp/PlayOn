using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Players.RequestData;
using PlayOn.Services.Data.Players.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.ParticipationRules;

namespace PlayOn.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<SportPreference> preferenceRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<PlayerRating> ratingRepository;

        public PlayerService(
            IRepository<ApplicationUser> userRepository,
            IRepository<Sport> sportRepository,
            IRepository<SportPreference> preferenceRepository,
            IRepository<MatchParticipant> participantRepository,
            IRepository<PlayerRating> ratingRepository)
        {
            this.userRepository = userRepository;
            this.sportRepository = sportRepository;
            this.preferenceRepository = preferenceRepository;
            this.participantRepository = participantRepository;
            this.ratingRepository = ratingRepository;
        }


        public async Task<ServiceResponse<PlayerProfileResponseModel>>
            GetProfileAsync(Guid userId)
        {
            var player = await userRepository
                .Query()
                .Where(x => x.Id == userId)
                .Select(PlayerDataBuilder.PlayerRowProjection())
                .SingleOrDefaultAsync();

            if (player == null || !player.IsActive)
            {
                return ServiceResponse<PlayerProfileResponseModel>.NotFound();
            }

            var attended = await CountAsync(userId, x => x.Status == ParticipantStatus.Attended);
            var noShow = await CountAsync(userId, x => x.Status == ParticipantStatus.NoShow);
            var lateCancel = await CountAsync(userId,
                x => x.Status == ParticipantStatus.Cancelled && x.IsLateCancel);

            var visibleRatings = ratingRepository
                .Query()
                .Where(x => x.RatedUserId == userId && !x.IsHidden);

            var ratingCount = await visibleRatings.CountAsync();
            var ratingSum = await visibleRatings.SumAsync(x => x.Score);

            var skills = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Sport.Name)
                .Select(PlayerDataBuilder.PlayerSkillProjection())
                .ToListAsync();

            var recentRatings = await visibleRatings
                .Where(x => x.Comment != null && x.Comment != "")
                .OrderByDescending(x => x.Id)
                .Take(RecentRatingsCount)
                .Select(PlayerDataBuilder.PlayerRatingProjection())
                .ToListAsync();

            var response = PlayerDataBuilder.PlayerToProfileResponseModel(
                player,
                attended,
                noShow,
                lateCancel,
                ratingCount,
                ratingSum,
                skills,
                recentRatings);

            return ServiceResponse<PlayerProfileResponseModel>.Success(response);
        }


        public async Task<ServiceResponse<PlayerEditResponseModel>>
            GetForEditAsync(Guid userId)
        {
            var player = await userRepository
                .Query()
                .Where(x => x.Id == userId)
                .Select(PlayerDataBuilder.PlayerRowProjection())
                .SingleOrDefaultAsync();

            if (player == null)
            {
                return ServiceResponse<PlayerEditResponseModel>.NotFound();
            }

            var preferences = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId)
                .ToDictionaryAsync(x => x.SportId, x => x.SkillLevel);

            var skills = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            var options = skills
                .Select(x => new PlayerSkillOptionResponseModel
                {
                    SportId = x.Id,
                    SportName = x.Name,
                    SkillLevel = preferences.TryGetValue(x.Id, out var level)
                        ? level
                        : null
                })
                .ToList();

            return ServiceResponse<PlayerEditResponseModel>
                .Success(PlayerDataBuilder.PlayerToEditResponseModel(player, options));
        }


        public async Task<ServiceResponse<Guid>>
            UpdateAsync(PlayerUpdateRequestData request)
        {
            var errors = ServiceRequestDataValidator.Validate(request);

            if (errors.Any())
            {
                return ServiceResponse<Guid>.Failure(errors);
            }

            var user = await userRepository
                .Query(asNoTracking: false)
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
            {
                return ServiceResponse<Guid>.NotFound();
            }

            var validSportIds = await sportRepository
                .Query()
                .Select(x => x.Id)
                .ToListAsync();

            var skillErrors = new List<ValidationResult>();
            var seen = new HashSet<Guid>();

            for (var i = 0; i < request.Skills.Count; i++)
            {
                var skill = request.Skills[i];

                if (!validSportIds.Contains(skill.SportId) || !seen.Add(skill.SportId))
                {
                    skillErrors.Add(new ValidationResult(
                        "Select a valid sport.",
                        new[] { $"Skills[{i}].SkillLevel" }));
                }
                else if (skill.SkillLevel.HasValue && !Enum.IsDefined(skill.SkillLevel.Value))
                {
                    skillErrors.Add(new ValidationResult(
                        "Select a valid skill level.",
                        new[] { $"Skills[{i}].SkillLevel" }));
                }
            }

            if (skillErrors.Count > 0)
            {
                return ServiceResponse<Guid>.Failure(skillErrors);
            }

            var existing = await preferenceRepository
                .Query(asNoTracking: false)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync();

            user.DisplayName = request.DisplayName.Trim();
            user.City = request.City.Trim();

            foreach (var skill in request.Skills)
            {
                var current = existing.FirstOrDefault(x => x.SportId == skill.SportId);

                if (skill.SkillLevel == null)
                {
                    if (current != null)
                    {
                        preferenceRepository.Remove(current);
                    }

                    continue;
                }

                if (current != null)
                {
                    current.SkillLevel = skill.SkillLevel.Value;
                }
                else
                {
                    await preferenceRepository.AddAsync(
                        PlayerDataBuilder.SkillToEntity(
                            request.UserId,
                            skill.SportId,
                            skill.SkillLevel.Value));
                }
            }

            await userRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(request.UserId);
        }


        private async Task<int> CountAsync(
            Guid userId,
            System.Linq.Expressions.Expression<Func<MatchParticipant, bool>> filter)
        {
            return await participantRepository
                .Query()
                .Where(x => x.UserId == userId)
                .CountAsync(filter);
        }
    }
}
