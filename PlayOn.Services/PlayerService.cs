using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Services.Contracts;
using PlayOn.Services.Data;
using PlayOn.Services.Data.Players;
using PlayOn.Services.Data.Players.RequestData;
using PlayOn.Services.Data.Players.ResponseModels;
using PlayOn.Services.DataBuilders;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
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
            GetProfileAsync(
                Guid userId,
                Guid targetProfileId)
        {
            var player = await GetPlayerRowAsync(targetProfileId, onlyActive: true);

            if (player == null)
            {
                return ServiceResponse<PlayerProfileResponseModel>.NotFound();
            }

            var attended = await CountParticipationsAsync(
                targetProfileId,
                x => x.Status == ParticipantStatus.Attended);

            var noShow = await CountParticipationsAsync(
                targetProfileId,
                x => x.Status == ParticipantStatus.NoShow);

            var lateCancel = await CountParticipationsAsync(
                targetProfileId,
                x => x.Status == ParticipantStatus.Cancelled && x.IsLateCancel);

            var visibleRatings = ratingRepository
                .Query()
                .Where(x => x.RatedUserId == targetProfileId && !x.IsHidden);

            var ratingCount = await visibleRatings.CountAsync();
            var ratingSum = await visibleRatings.SumAsync(x => x.Score);

            var skills = await preferenceRepository
                .Query()
                .Where(x => x.UserId == targetProfileId)
                .OrderBy(x => x.Sport.Name)
                .Select(PlayerDataBuilder.PlayerSkillProjection())
                .ToListAsync();

            var recentRatings = await visibleRatings
                .Where(x => x.Comment != null && x.Comment != "")
                .OrderByDescending(x => x.Id)
                .Take(RecentRatingsCount)
                .Select(PlayerDataBuilder.PlayerRatingProjection())
                .ToListAsync();

            var isOwnProfile = userId != Guid.Empty
                && targetProfileId == userId;

            var responseData = PlayerDataBuilder.PlayerToProfileResponseModel(
                player,
                attended,
                noShow,
                lateCancel,
                ratingCount,
                ratingSum,
                skills,
                recentRatings,
                isOwnProfile);

            return ServiceResponse<PlayerProfileResponseModel>
                .Success(responseData);
        }


        public async Task<ServiceResponse<PlayerEditResponseModel>>
            GetForEditAsync(Guid userId)
        {
            var player = await GetPlayerRowAsync(userId, onlyActive: false);

            if (player == null)
            {
                return ServiceResponse<PlayerEditResponseModel>.NotFound();
            }

            var skillOptionsResponse = await GetSkillOptionsAsync(userId);

            if (!skillOptionsResponse.IsSuccess)
            {
                return ServiceResponse<PlayerEditResponseModel>
                    .Failure(skillOptionsResponse.Errors);
            }

            var responseData = PlayerDataBuilder.PlayerToEditResponseModel(
                player,
                skillOptionsResponse.ResponseData);

            return ServiceResponse<PlayerEditResponseModel>
                .Success(responseData);
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

            var skillErrors = await ValidateSkillsAsync(request.Skills);

            if (skillErrors.Any())
            {
                return ServiceResponse<Guid>.Failure(skillErrors);
            }

            user.DisplayName = request.DisplayName.Trim();
            user.City = request.City.Trim();

            await ApplySkillsAsync(request.UserId, request.Skills);

            await userRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(request.UserId);
        }


        private async Task<PlayerRow?> GetPlayerRowAsync(
            Guid userId,
            bool onlyActive)
        {
            var query = userRepository
                .Query()
                .Where(x => x.Id == userId);

            if (onlyActive)
            {
                query = query.Where(x => x.IsActive);
            }

            return await query
                .Select(PlayerDataBuilder.PlayerRowProjection())
                .SingleOrDefaultAsync();
        }

        private async Task<ServiceResponse<IReadOnlyList<PlayerSkillOptionResponseModel>>>
            GetSkillOptionsAsync(Guid userId)
        {
            var skillLevels = await preferenceRepository
                .Query()
                .Where(x => x.UserId == userId)
                .ToDictionaryAsync(x => x.SportId, x => x.SkillLevel);

            var sports = await sportRepository
                .Query()
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();

            IReadOnlyList<PlayerSkillOptionResponseModel> skillOptions = sports
                .Select(x => new PlayerSkillOptionResponseModel
                {
                    SportId = x.Id,
                    SportName = x.Name,
                    SkillLevel = skillLevels.TryGetValue(x.Id, out var level)
                        ? level
                        : null
                })
                .ToList();

            return ServiceResponse<IReadOnlyList<PlayerSkillOptionResponseModel>>
                .Success(skillOptions);
        }

        private async Task<IEnumerable<ValidationResult>> ValidateSkillsAsync(
            IReadOnlyList<PlayerSkillRequestData> skills)
        {
            var validSportIds = (await sportRepository
                .Query()
                .Select(x => x.Id)
                .ToListAsync())
                .ToHashSet();

            var errors = new List<ValidationResult>();
            var seenSportIds = new HashSet<Guid>();

            for (var i = 0; i < skills.Count; i++)
            {
                var skill = skills[i];
                var member = $"Skills[{i}].SkillLevel";

                if (!validSportIds.Contains(skill.SportId)
                    || !seenSportIds.Add(skill.SportId))
                {
                    errors.Add(new ValidationResult(
                        "Select a valid sport.",
                        new[] { member }));
                }
                else if (skill.SkillLevel.HasValue
                    && !Enum.IsDefined(skill.SkillLevel.Value))
                {
                    errors.Add(new ValidationResult(
                        "Select a valid skill level.",
                        new[] { member }));
                }
            }

            return errors;
        }

        private async Task ApplySkillsAsync(
            Guid userId,
            IEnumerable<PlayerSkillRequestData> skills)
        {
            var currentPreferences = await preferenceRepository
                .Query(asNoTracking: false)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var skill in skills)
            {
                var current = currentPreferences
                    .FirstOrDefault(x => x.SportId == skill.SportId);

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

                    continue;
                }

                await preferenceRepository.AddAsync(
                    PlayerDataBuilder.SkillToEntity(
                        userId,
                        skill.SportId,
                        skill.SkillLevel.Value));
            }
        }

        private async Task<int> CountParticipationsAsync(
            Guid userId,
            Expression<Func<MatchParticipant, bool>> filter)
        {
            return await participantRepository
                .Query()
                .Where(x => x.UserId == userId)
                .CountAsync(filter);
        }
    }
}
