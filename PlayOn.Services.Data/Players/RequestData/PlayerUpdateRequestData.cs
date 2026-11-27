using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Services.Data.Players.RequestData
{
    /// <summary>
    /// Request data for updating the profile of a player.
    /// </summary>
    public class PlayerUpdateRequestData
    {
        /// <summary>
        /// Identifier of the user whose profile is updated.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// New display name of the player.
        /// </summary>
        [Required]
        [StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// New city of the player.
        /// </summary>
        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        /// <summary>
        /// Skill levels of the player per sport.
        /// </summary>
        public List<PlayerSkillRequestData> Skills { get; set; } = [];
    }

    /// <summary>
    /// The skill level of a player in a single sport within a <see cref="PlayerUpdateRequestData"/>.
    /// </summary>
    public class PlayerSkillRequestData
    {
        /// <summary>
        /// Identifier of the sport.
        /// </summary>
        public Guid SportId { get; set; }

        /// <summary>
        /// Skill level of the player in the sport, or <c>null</c> to clear it.
        /// </summary>
        public SkillLevel? SkillLevel { get; set; }
    }
}
