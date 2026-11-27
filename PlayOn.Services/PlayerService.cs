using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using PlayOn.Data.Models;
using PlayOn.Data.RepositoryContract;
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
    /// <summary>
    /// Provides player profiles, profile editing data and profile updates including sport skill levels.
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<ApplicationUser> userRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<SportPreference> preferenceRepository;
        private readonly IRepository<MatchParticipant> participantRepository;
        private readonly IRepository<PlayerRating> ratingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for users.</param>
        /// <param name="sportRepository">Repository for sports.</param>
        /// <param name="preferenceRepository">Repository for sport skill preferences.</param>
        /// <param name="participantRepository">Repository for match participants.</param>
        /// <param name="ratingRepository">Repository for player ratings.</param>
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

        /// <summary>
        /// Builds the public profile of an active player, including attendance, no-show and late-cancel counts, average of visible ratings, skills and recent rated comments.
        /// </summary>
        /// <param name="userId">The identifier of the viewing user; used to flag the player's own profile.</param>
        /// <param name="targetProfileId">The identifier of the player whose profile is requested.</param>
        /// <returns>A service response with the profile, or not found if the player does not exist or is inactive.</returns>
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
                .OrderBy(x => x.MatchId)
                .ThenBy(x => x.RaterId)
                .ThenBy(x => x.RatedUserId)
                .Take(RecentRatingsCount)
                .Select(PlayerDataBuilder.PlayerRatingProjection())
                .ToListAsync();

            var isOwnProfile = userId != Guid.Empty && targetProfileId == userId;

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

            return ServiceResponse<PlayerProfileResponseModel>.Success(responseData);
        }

        /// <summary>
        /// Loads the data needed to edit a player's profile, including a skill option for every sport.
        /// </summary>
        /// <param name="userId">The identifier of the user to edit.</param>
        /// <returns>A service response with the edit model, or not found if the user does not exist.</returns>
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

            return ServiceResponse<PlayerEditResponseModel>.Success(responseData);
        }

        /// <summary>
        /// Updates the user's display name, city and sport skill levels after validating the request and the skills.
        /// A null skill level removes the preference for that sport.
        /// </summary>
        /// <param name="request">The update request.</param>
        /// <returns>A service response with the user identifier, validation errors, or not found if the user does not exist.</returns>
        public async Task<ServiceResponse<Guid>>
            UpdateAsync(PlayerUpdateRequestData request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var errors = ServiceRequestDataValidator.Validate(request).ToList();
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

            var skills = request.Skills;

            var skillErrors = await ValidateSkillsAsync(skills);
            if (skillErrors.Any())
            {
                return ServiceResponse<Guid>.Failure(skillErrors);
            }

            user.DisplayName = request.DisplayName.Trim();
            user.City = request.City.Trim();

            await ApplySkillsAsync(request.UserId, skills);

            await userRepository.SaveChangesAsync();

            return ServiceResponse<Guid>.Success(request.UserId);
        }

        /// <summary>
        /// Loads the basic player row for a user.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="onlyActive">Whether to restrict the lookup to active users.</param>
        /// <returns>The player row, or <c>null</c> if not found.</returns>
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

        /// <summary>
        /// Builds a skill option for every sport, filled with the user's current skill level where one is set.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>A service response with the skill options ordered by sport name.</returns>
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

        /// <summary>
        /// Validates the submitted skills: each sport must exist, appear only once, and have a defined skill level if one is given.
        /// </summary>
        /// <param name="skills">The submitted skills.</param>
        /// <returns>The validation errors, empty if the skills are valid.</returns>
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

                if (!validSportIds.Contains(skill.SportId))
                {
                    errors.Add(new ValidationResult(
                        "Select a valid sport.",
                        new[] { member }));
                }
                else if (!seenSportIds.Add(skill.SportId))
                {
                    errors.Add(new ValidationResult(
                        "Duplicate sport selection is not allowed.",
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

        /// <summary>
        /// Applies the submitted skills to the user's preferences by updating existing ones, adding new ones and removing those with no skill level.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="skills">The submitted skills.</param>
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
                    .SingleOrDefault(x => x.SportId == skill.SportId);

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

        /// <summary>
        /// Counts the user's participations that satisfy the filter.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="filter">The additional condition participations must satisfy.</param>
        /// <returns>The number of matching participations.</returns>
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