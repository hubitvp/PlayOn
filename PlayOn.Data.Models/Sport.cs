using Microsoft.EntityFrameworkCore;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;


namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a sport that can be played on the platform.
    /// </summary>
    public class Sport
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        [Key]
        [Comment("Unique identifier of the sport")]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of the sport.
        /// </summary>
        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Name of the sport")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Icon used to visually represent the sport.
        /// </summary>
        [Required]
        [Comment("Icon used to visually represent the sport")]
        public SportIcon Icon { get; set; } = SportIcon.TrophyDefault;

        /// <summary>
        /// Default minimum number of players for a match of this sport.
        /// </summary>
        [Required]
        [Comment("Default minimum number of players for a match of this sport")]
        public int DefaultMinPlayers { get; set; }

        /// <summary>
        /// Default maximum number of players for a match of this sport.
        /// </summary>
        [Required]
        [Comment("Default maximum number of players for a match of this sport")]
        public int DefaultMaxPlayers { get; set; }

        /// <summary>
        /// Collection of courts on which this sport can be played.
        /// </summary>
        public virtual ICollection<CourtSport> Courts { get; set; }
            = new List<CourtSport>();

        /// <summary>
        /// Collection of user preferences for this sport.
        /// </summary>
        public virtual ICollection<SportPreference> Preferences { get; set; }
            = new List<SportPreference>();

        /// <summary>
        /// Collection of events for this sport.
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
