using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a user's preferred sport together with their skill level in it.
    /// </summary>
    public class SportPreference
    {
        /// <summary>
        /// Identifier of the user who owns the preference.
        /// </summary>
        [Required]
        [Comment("Identifier of the user who owns the preference")]
        public Guid UserId { get; set; }

        /// <summary>
        /// The user who owns the preference.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Identifier of the preferred sport.
        /// </summary>
        [Required]
        [Comment("Identifier of the preferred sport")]
        public Guid SportId { get; set; }

        /// <summary>
        /// The preferred sport.
        /// </summary>
        [ForeignKey(nameof(SportId))]
        public Sport Sport { get; set; } = null!;

        /// <summary>
        /// Skill level of the user in the preferred sport.
        /// </summary>
        [Comment("Skill level of the user in the preferred sport")]
        public SkillLevel SkillLevel { get; set; }
    }
}
