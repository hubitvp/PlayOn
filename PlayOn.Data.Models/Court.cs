using PlayOn.Common.EntityConstants;
using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;


namespace PlayOn.Data.Models
{
    public class Court
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(CourtConstants.NameMaxLength)]
        public string Name { get; set; } = null!;


        [Required]
        public Surface Surface { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal HourlyPrice { get; set; }

        public virtual Venue? Venue { get; set; }

        public virtual ICollection<CourtSport> Sports { get; set; }
            = new List<CourtSport>();


        public virtual ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}
