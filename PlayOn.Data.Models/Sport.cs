using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.SportConstants;


namespace PlayOn.Data.Models
{
    public class Sport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; } = null!;


        [MaxLength(IconClassMaxLenght)]
        public string? IconClass { get; set; }

        [Required]
        public int DefaultMinPlayers { get; set; }


        [Required]
        public int DefaultMaxPlayers { get; set; }

        public virtual ICollection<SportPreference> SportPreferences { get; set; }
            = new List<SportPreference>();

        public virtual ICollection<Court> Courts { get; set; }
            = new List<Court>();
    }
}
