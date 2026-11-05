using System.ComponentModel.DataAnnotations;
using PlayOn.Common.Enums;
using static PlayOn.Common.EntityConstants.SportConstants;


namespace PlayOn.Data.Models
{
    public class Sport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;


        [Required]
        public SportIcon Icon { get; set; } = SportIcon.TrophyDefault;

        [Required]
        public int DefaultMinPlayers { get; set; }


        [Required]
        public int DefaultMaxPlayers { get; set; }



        public virtual ICollection<CourtSport> Courts { get; set; }
            = new List<CourtSport>();


        public virtual ICollection<SportPreference> Preferences { get; set; }
            = new List<SportPreference>();


        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
